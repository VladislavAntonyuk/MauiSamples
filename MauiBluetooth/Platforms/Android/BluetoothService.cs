namespace MauiBluetooth;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Java.Lang;
using Java.Util;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Android;

public class BluetoothService : IBluetoothService
{
	private readonly IAdapter adapter;

	static UUID? _myUuidSecure = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");
	private readonly BluetoothManager manager;
	BluetoothSocket? socket;

	public BluetoothService(IAdapter adapter)
	{
		this.adapter = adapter;
		manager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService)!;
	}

	/// <inheritdoc />
	public IDevice[] GetConnectedDevices()
	{
		var bluetoothAdapter = PrepareAdapter();

		if (bluetoothAdapter.BondedDevices?.Count > 0)
		{
			return bluetoothAdapter.BondedDevices.Select(d => new Device((Adapter)adapter, d, null, 0)).ToArray();
		}

		return Array.Empty<IDevice>();
	}

	/// <inheritdoc />
	public Task Connect(string deviceName)
	{
		var bluetoothAdapter = PrepareAdapter();
		bluetoothAdapter.CancelDiscovery();
		var device = bluetoothAdapter.BondedDevices?.FirstOrDefault(d => d.Name == deviceName);

		socket = device?.CreateRfcommSocketToServiceRecord(_myUuidSecure);
		if (socket is null)
		{
			return Task.CompletedTask;
		}

		//var thread = new ConnectThread(socket);
		//thread.Start();
		try
		{
			socket.Connect();
		}
		catch (Exception)
		{
		}
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Task Disconnect()
	{
		if (socket is not null && socket.IsConnected)
		{
			socket?.Close();
			socket?.Dispose();
			socket = null;
		}

		return Task.CompletedTask;
	}

	public async Task Send(string deviceName, byte[] content)
	{
		if (socket is null || !socket.IsConnected)
		{
			await Connect(deviceName);
		}

		if (socket?.OutputStream is null)
		{
			return;
		}

		await socket.OutputStream.WriteAsync(content, 0, content.Length);
	}

	public async Task<byte[]> Receive(string deviceName)
	{
		if (socket is null || !socket.IsConnected)
		{
			await Connect(deviceName);
		}

		if (socket?.InputStream is null)
		{
			return Array.Empty<byte>();
		}

		byte[] content = new byte[1024];
		await socket.InputStream.ReadAsync(content, 0, content.Length);
		return content;

	}

	BluetoothAdapter PrepareAdapter()
	{
		var adapter = manager.Adapter;
		if (adapter is null)
		{
			throw new Exception("No Bluetooth adapter found.");
		}

		if (!adapter.IsEnabled)
		{
			Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
			Application.Context.StartActivity(enableBtIntent);
		}

		return adapter;
	}
}