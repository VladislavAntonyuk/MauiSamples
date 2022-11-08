namespace MauiBluetooth;

using CommunityToolkit.Maui;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();
		builder.UseMauiCommunityToolkit();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<IBluetoothLE>(CrossBluetoothLE.Current);
		builder.Services.AddSingleton<IAdapter>(CrossBluetoothLE.Current.Adapter);
		builder.Services.AddSingleton<IBluetoothService, BluetoothService>();
		return builder.Build();
	}
}