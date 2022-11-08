namespace MauiBluetooth;

using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Plugin.BLE.Abstractions.Contracts;

public partial class DetailsPage : Popup
{
	private readonly IDevice device;
	private readonly IBluetoothService bluetoothService;

	public DetailsPage(IDevice device, IBluetoothService bluetoothService)
	{
		this.device = device;
		this.bluetoothService = bluetoothService;
		InitializeComponent();
		BindingContext = this;
		Size = new Size(200, 500);
	}

	private async void GetServices(object sender, EventArgs e)
	{
		var services = await device.GetServicesAsync();
		var service = services.First();
		var characteristics = await service.GetCharacteristicsAsync();
		var characteristic = characteristics.First();
		//var bytes = await characteristic.ReadAsync();
		characteristic.ValueUpdated += (o, args) =>
		{
			var bytes = args.Characteristic.Value;
		};

		await characteristic.StartUpdatesAsync();
		var descriptors = await characteristic.GetDescriptorsAsync();
		var descriptor = descriptors.First();
		var bytes = await descriptor.ReadAsync();
		await descriptor.WriteAsync(bytes);
	}

	private async void SendMessage(object sender, EventArgs e)
	{
		await bluetoothService.Connect(device.Name);
		await bluetoothService.Send(device.Name, Encoding.Default.GetBytes("test"));

		var data = await bluetoothService.Receive(device.Name);
		await Toast.Make(Encoding.Default.GetString(data)).Show();
		await bluetoothService.Disconnect();
	}
}