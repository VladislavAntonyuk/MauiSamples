namespace MauiBluetooth;

using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

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
	private readonly IBluetoothService bluetoothService;

	[ObservableProperty]
	private ObservableCollection<string> devices = new();
	public MainPageViewModel(IBluetoothService bluetoothService)
	{
		this.bluetoothService = bluetoothService;
		devices = new ObservableCollection<string>(bluetoothService.GetConnectedDevices());
	}
}