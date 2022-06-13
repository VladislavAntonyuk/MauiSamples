namespace MauiBluetooth;

public interface IBluetoothService
{
	string[] GetConnectedDevices();
	
	Task Send(string deviceName, byte[] content);
}