namespace MauiBluetooth;

using Plugin.BLE.Abstractions.Contracts;

public interface IBluetoothService
{
	IDevice[] GetConnectedDevices();

	Task Connect(string deviceName);
	Task Disconnect();
	Task Send(string deviceName, byte[] content);
	Task<byte[]> Receive(string deviceName);
}