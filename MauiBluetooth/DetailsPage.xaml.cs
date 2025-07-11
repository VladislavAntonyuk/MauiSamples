﻿namespace MauiBluetooth;

using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Plugin.BLE.Abstractions.Contracts;

public partial class DetailsPage : Popup
{
	public IDevice Device { get; }
	private readonly IBluetoothService bluetoothService;

	public DetailsPage(IDevice device, IBluetoothService bluetoothService)
	{
		Device = device;
		this.bluetoothService = bluetoothService;
		InitializeComponent();
		BindingContext = this;
		WidthRequest = 200;
		HeightRequest = 500;
	}

	private async void GetServices(object sender, EventArgs e)
	{
		var services = await Device.GetServicesAsync();
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
		await bluetoothService.Connect(Device.Name);
		await bluetoothService.Send(Device.Name, Encoding.Default.GetBytes("test"));

		var data = await bluetoothService.Receive(Device.Name);
		await Toast.Make(Encoding.Default.GetString(data)).Show();
		await bluetoothService.Disconnect();
	}
}