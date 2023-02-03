namespace MauiBluetooth;

using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

public partial class MainPageViewModel : ObservableObject
{
	private readonly IAdapter adapter;
	private readonly IBluetoothService bluetoothService;

	[ObservableProperty]
	private ObservableCollection<IDevice> devices = new();

	public MainPageViewModel(IAdapter adapter, IBluetoothService bluetoothService)
	{
		this.adapter = adapter;
		this.bluetoothService = bluetoothService;
	}

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
			if (Application.Current?.MainPage != null)
			{
				await Application.Current.MainPage.ShowPopupAsync(new DetailsPage(device, bluetoothService));
			}
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