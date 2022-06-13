namespace MauiBluetooth;

using Android.Bluetooth;
using Android.Content;
using com.xamarin.samples.bluetooth.bluetoothchat;
using Java.Util;

public class BluetoothService : IBluetoothService
{
	/// <summary>
	/// The standard UDID for SSP
	/// </summary>
	private const string SspUuid = "00001101-0000-1000-8000-00805f9b34fb";
	private BluetoothManager manager;

	public BluetoothService()
	{
		manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Context.BluetoothService)!;
	}

	/// <inheritdoc />
	public string[] GetConnectedDevices()
	{
		var adapter = PrepareAdapter();

		if (adapter.IsEnabled)
		{
			if (adapter.BondedDevices?.Count > 0)
			{
				return adapter.BondedDevices.Where(x => !string.IsNullOrEmpty(x.Name)).Select(d => d.Name!).ToArray();
			}
		}
		else
		{
			throw new Exception("Bluetooth is not enabled on device");
		}

		return Array.Empty<string>();
	}

	/// <inheritdoc />
	private async Task<BluetoothSocket?> Connect(string deviceName)
	{
		var adapter = PrepareAdapter();
		var device = adapter.BondedDevices?.FirstOrDefault(d => d.Name == deviceName);

		var socket = device?.CreateRfcommSocketToServiceRecord(UUID.FromString(SspUuid));
		if (socket is null)
		{
			return null;
		}

		await socket.ConnectAsync();
		return socket;
	}

	public async Task Send(string deviceName, byte[] content)
	{
		var socket = await Connect(deviceName);

		if (socket?.OutputStream is null)
		{
			return;
		}

		await socket.OutputStream.WriteAsync(content, 0, content.Length);
	}

	public async Task Receive(string deviceName, byte[] content)
	{
		var socket = await Connect(deviceName);

		if (socket?.InputStream is null)
		{
			return;
		}

		await socket.InputStream.ReadAsync(content, 0, content.Length);
	}

	BluetoothAdapter PrepareAdapter()
	{
		var adapter = manager.Adapter;
		if (adapter == null)
		{
			throw new Exception("No Bluetooth adapter found.");
		}

		if (!adapter.IsEnabled)
		{
			Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
			Android.App.Application.Context.StartActivity(enableBtIntent);
		}

		return adapter;
	}
}