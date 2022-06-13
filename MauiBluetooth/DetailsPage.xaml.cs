namespace MauiBluetooth;

using CommunityToolkit.Maui.Views;
using Plugin.BLE.Abstractions.Contracts;

public partial class DetailsPage : Popup
{
	private readonly IDevice device;

	public DetailsPage(IDevice device)
	{
		this.device = device;
		InitializeComponent();
	}

	async Task GetServices()
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
}