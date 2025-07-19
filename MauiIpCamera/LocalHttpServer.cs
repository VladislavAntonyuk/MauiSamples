namespace MauiIpCamera;

using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class LocalHttpServer(IPAddress ipAddress, int port, int frequency)
{
	private TcpListener? listener;
	private byte[]? currentMjpegFrame;
	private long mjpegFrameTimestamp;
	private byte[]? videoStreamBytes;
	private readonly ConcurrentDictionary<NetworkStream, byte> activeClients = new();

	public void SetMjpegStream(Stream stream)
	{
		if (stream.Length == 0 || activeClients.IsEmpty)
		{
			return;
		}

		var buffer = new byte[stream.Length];
		stream.Position = 0;
		stream.ReadExactly(buffer, 0, buffer.Length);

		currentMjpegFrame = buffer;
		mjpegFrameTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
	}

	public void SetVideoStream(MemoryStream stream)
	{
		videoStreamBytes = stream.ToArray();
	}

	public Task StartAsync(int maxConnectionsCount, CancellationToken token)
	{
		listener = new TcpListener(ipAddress, port);
		listener.Start(maxConnectionsCount);

		return AcceptClientsAsync(token);
	}

	private async Task AcceptClientsAsync(CancellationToken token)
	{
		if (listener is null)
		{
			return;
		}

		while (!token.IsCancellationRequested)
		{
			try
			{
				var client = await listener.AcceptTcpClientAsync(token);
				_ = HandleClientAsync(client, token);
			}
			catch (OperationCanceledException) when (token.IsCancellationRequested)
			{
				break;
			}
		}
	}

	public void Stop()
	{
		listener?.Stop();
		activeClients.Clear();
		currentMjpegFrame = null;
		videoStreamBytes = null;
	}

	private async Task HandleClientAsync(TcpClient client, CancellationToken serverToken)
	{
		client.NoDelay = true;
		client.SendBufferSize = 65536;

		try
		{
			await using var stream = client.GetStream();
			using var reader = new StreamReader(stream, Encoding.UTF8, false, 1024, leaveOpen: true);

			var requestLine = await reader.ReadLineAsync(serverToken);
			if (string.IsNullOrWhiteSpace(requestLine))
			{
				return;
			}

			// Skip headers
			while (!string.IsNullOrWhiteSpace(await reader.ReadLineAsync(serverToken))) { }

			// Parse request
			var parts = requestLine.Split(' ');
			if (parts.Length < 2)
			{
				return;
			}

			var method = parts[0].ToUpperInvariant();
			var path = parts[1];

			if (method == "GET")
			{
				if (path.StartsWith("/mjpeg"))
				{
					activeClients.TryAdd(stream, 0);

					try
					{
						await SendMjpegStreamAsync(stream, serverToken);
					}
					finally
					{
						activeClients.TryRemove(stream, out _);
					}
				}
				else if (path.StartsWith("/stream"))
				{
					await SendVideoStreamAsync(stream, serverToken);
				}
				else if (path.StartsWith("/player"))
				{
					await SendPlayerAsync(stream, serverToken);
				}
				else
				{
					const string errorResponse = "HTTP/1.1 404 Not Found\r\n\r\nPage not found";
					await stream.WriteAsync(Encoding.UTF8.GetBytes(errorResponse), serverToken);
				}
			}
			else
			{
				const string errorResponse = "HTTP/1.1 405 Method Not Allowed\r\n\r\nOnly GET method is supported";
				await stream.WriteAsync(Encoding.UTF8.GetBytes(errorResponse), serverToken);
			}

		}
		catch
		{
			// Ignore client errors
		}
		finally
		{
			client.Close();
		}
	}

	private async Task SendMjpegStreamAsync(NetworkStream stream, CancellationToken token)
	{
		var boundary = $"mjpeg-{Guid.NewGuid():N}";
		var header = $"HTTP/1.1 200 OK\r\n" +
					 $"Content-Type: multipart/x-mixed-replace; boundary={boundary}\r\n" +
					 "Connection: keep-alive\r\n" +
					 "Cache-Control: no-cache\r\n" +
					 "Pragma: no-cache\r\n\r\n";

		await stream.WriteAsync(Encoding.UTF8.GetBytes(header), token);

		var lastTimestamp = 0L;

		while (!token.IsCancellationRequested)
		{
			var frame = currentMjpegFrame;
			var timestamp = mjpegFrameTimestamp;

			if (frame == null || timestamp <= lastTimestamp)
			{
				await Task.Delay(frequency, token);
				continue;
			}

			var partHeader = $"--{boundary}\r\n" +
							 $"Content-Type: image/jpeg\r\n" +
							 $"Content-Length: {frame.Length}\r\n\r\n";

			await stream.WriteAsync(Encoding.UTF8.GetBytes(partHeader), token);
			await stream.WriteAsync(frame, token);
			await stream.FlushAsync(token);

			lastTimestamp = timestamp;
		}
	}

	private async Task SendVideoStreamAsync(NetworkStream stream, CancellationToken token)
	{
		if (videoStreamBytes == null)
		{
			const string errorResponse = "HTTP/1.1 404 Not Found\r\n\r\nVideo stream not available";
			await stream.WriteAsync(Encoding.UTF8.GetBytes(errorResponse), token);
			return;
		}

		var header = $"HTTP/1.1 200 OK\r\n" +
					 $"Content-Type: video/mp4\r\n" +
					 $"Content-Length: {videoStreamBytes.Length}\r\n" +
					 "Connection: close\r\n" +
					 "Cache-Control: no-cache\r\n" +
					 "Pragma: no-cache\r\n\r\n";

		await stream.WriteAsync(Encoding.UTF8.GetBytes(header), token);
		await stream.WriteAsync(videoStreamBytes, token);
		await stream.FlushAsync(token);

		stream.Close();
	}


	private async Task SendPlayerAsync(NetworkStream stream, CancellationToken token)
	{
		if (videoStreamBytes == null)
		{
			const string errorResponse = "HTTP/1.1 404 Not Found\r\n\r\nVideo stream not available";
			await stream.WriteAsync(Encoding.UTF8.GetBytes(errorResponse), token);
			return;
		}

		var header = $"HTTP/1.1 200 OK\r\n" +
					 $"Content-Type: text/html\r\n" +
					 "Connection: close\r\n" +
					 "Cache-Control: no-cache\r\n" +
					 "Pragma: no-cache\r\n\r\n";

		await stream.WriteAsync(Encoding.UTF8.GetBytes(header), token);

		Guid viewerId = Guid.NewGuid();
		await stream.WriteAsync(Encoding.UTF8.GetBytes(
									$$"""
			                          		<!DOCTYPE html>
			                          		<html lang="en">
			                          		<head>
			                          		    <meta charset="UTF-8">
			                          		    <meta name="viewport" content="width=device-width, initial-scale=1.0">
			                          		    <title>MAUI IP Camera Video player</title>
			                          		    <style>
			                          		        body { margin: 0; padding: 0; display: flex; justify-content: center; align-items: center; height: 100vh; background-color: #000; color: #fff }
			                          		        video { max-width: 100%; max-height: 100vh; }
			                          		        .controls { position: fixed; bottom: 10px; width: 100%; text-align: center; }
			                          		        button { background: rgba(255,255,255,0.7); border: none; padding: 8px 16px; border-radius: 4px; margin: 0 5px; }
			                          		    </style>
			                          		</head>
			                          		<body>
			                          		    <video id="videoPlayer" controls playsinline webkit-playsinline></video>
			                          		    <div class="controls">
			                          			    <p id="info">
			                          					Viewer Id: {{viewerId}}<br>
			                          					Device info: {{DeviceInfo.Current.Model}}<br>
			                          			    </p>
			                          			    <p id="status"></p>
			                          		        <button id="refreshBtn">Refresh Video</button>
			                          		    </div>

			                          		    <script>
			                          		        const viewerId = '{{viewerId}}';
			                          		        const status = document.getElementById('status');
			                          		        const info = document.getElementById('info');
			                          		        const video = document.getElementById('videoPlayer');
			                          		        const refreshBtn = document.getElementById('refreshBtn');
			                          		        let timestamp = new Date().getTime();

			                          		        function loadVideo() {
			                          		            timestamp = new Date().getTime();
			                          		            video.src = `/stream?t=${timestamp}`;
			                          		            video.load();
			                          		            video.play().catch(err => {
			                          		                status.innerText = 'Error playing video: ' + err;
			                          		            });
			                          		        }

			                          		        loadVideo();

			                          		        video.addEventListener('ended', loadVideo);
			                          		        video.addEventListener('error', (e) => {
			                          		            status.innerText = 'Video error: ' + e;
			                          		            setTimeout(loadVideo, 2000);
			                          		        });

			                          		        refreshBtn.addEventListener('click', loadVideo);
			                          		    </script>
			                          		</body>
			                          		</html>
			                          """), token);

		await stream.FlushAsync(token);
		stream.Close();
	}
}