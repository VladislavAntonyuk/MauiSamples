namespace MauiBluetooth;

using Plugin.BLE.Abstractions.Contracts;

public class BluetoothService : IBluetoothService
{
	private readonly IAdapter adapter;

	public BluetoothService(IAdapter adapter)
	{
		this.adapter = adapter;
	}

	/// <inheritdoc />
	public IDevice[] GetConnectedDevices()
	{
		return adapter.ConnectedDevices.ToArray();
	}

	/// <inheritdoc />
	public Task Connect(string deviceName)
	{
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Task Disconnect()
	{
		return Task.CompletedTask;
	}

	public async Task Send(string deviceName, byte[] content)
	{
		await Task.CompletedTask;
		return;
	}

	public async Task<byte[]> Receive(string deviceName)
	{
		await Task.CompletedTask;
		return Array.Empty<byte>();
	}
}