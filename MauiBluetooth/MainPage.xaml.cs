namespace MauiBluetooth;

using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = mainPageViewModel;
	}
}

public partial class MainPageViewModel(IAdapter adapter, IBluetoothService bluetoothService) : ObservableObject
{
	[ObservableProperty]
	public partial ObservableCollection<IDevice> Devices { get; set; } = new();

	[RelayCommand]
	async Task ScanDevices(CancellationToken cancellationToken)
	{
		Devices.Clear();
		adapter.DeviceDiscovered += (sender, args) =>
		{
			if (!Devices.Contains(args.Device))
			{
				Devices.Add(args.Device);
			}
		};
		await adapter.StartScanningForDevicesAsync(cancellationToken: cancellationToken);
		foreach (var device in bluetoothService.GetConnectedDevices())
		{
			Devices.Add(device);
		}
	}

	[RelayCommand]
	async Task Connect(IDevice device, CancellationToken cancellationToken)
	{
		try
		{
			await adapter.ConnectToDeviceAsync(device, cancellationToken: cancellationToken);
			var page = Application.Current?.Windows.LastOrDefault()?.Page;
			if (page is null)
			{
				ArgumentNullException.ThrowIfNull(page);
			}

			await page.ShowPopupAsync(new DetailsPage(device, bluetoothService), token: cancellationToken);

		}
		catch (DeviceConnectionException e)
		{
			await Toast.Make(e.Message).Show(cancellationToken);
		}
		catch (Exception e)
		{
			await Toast.Make(e.Message).Show(cancellationToken);
		}
	}
}