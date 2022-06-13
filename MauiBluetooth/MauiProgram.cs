namespace MauiBluetooth;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
#if ANDROID
		builder.Services.AddSingleton<IBluetoothService, BluetoothService>();
#endif
		return builder.Build();
	}
}