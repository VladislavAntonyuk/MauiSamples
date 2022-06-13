namespace MauiBluetooth;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = mainPageViewModel;
	}
}

public partial class MainPageViewModel : ObservableObject
{
	private readonly IBluetoothLE ble;
	private readonly IAdapter adapter;

	[ObservableProperty]
	private ObservableCollection<IDevice> devices = new();

	public MainPageViewModel(IBluetoothLE ble, IAdapter adapter)
	{
		this.ble = ble;
		this.adapter = adapter;
		ScanDevicesCommand.Execute(null);

	}

	[RelayCommand]
	async Task ScanDevices(CancellationToken cancellationToken)
	{
		adapter.DeviceDiscovered += (s,a) => devices.Add(a.Device);
		await adapter.StartScanningForDevicesAsync(cancellationToken: cancellationToken);
	}

	[RelayCommand]
	async Task Connect(IDevice device, CancellationToken cancellationToken)
	{
		try
		{
			await adapter.ConnectToDeviceAsync(device, cancellationToken: cancellationToken);
			if (Application.Current?.MainPage != null)
			{
				await Application.Current.MainPage.ShowPopupAsync(new DetailsPage(device));
			}
		}
		catch(DeviceConnectionException e)
		{
			await Toast.Make(e.Message).Show(cancellationToken);
		}
	}
}