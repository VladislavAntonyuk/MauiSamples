namespace MauiIpCamera;

using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class LocalHttpServer(IPAddress ipAddress, int port, int frequency)
{
	private TcpListener? listener;
	private byte[]? currentFrame;
	private long frameTimestamp;
	private readonly ConcurrentDictionary<NetworkStream, byte> activeClients = new();

	public void SetStream(Stream stream)
	{
		if (stream.Length == 0 || activeClients.IsEmpty)
		{
			return;
		}

		var buffer = new byte[stream.Length];
		stream.Position = 0;
		stream.ReadExactly(buffer, 0, buffer.Length);

		currentFrame = buffer;
		frameTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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
		currentFrame = null;
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

			if (method != "GET")
			{
				const string errorResponse = "HTTP/1.1 405 Method Not Allowed\r\n\r\nOnly GET method is supported";
				await stream.WriteAsync(Encoding.UTF8.GetBytes(errorResponse), serverToken);
				return;
			}

			if (path.StartsWith("/stream"))
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
			else
			{
				const string errorResponse = "HTTP/1.1 404 Not Found\r\n\r\nPage not found";
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
			var frame = currentFrame;
			var timestamp = frameTimestamp;

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
}