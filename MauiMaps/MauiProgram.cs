namespace MauiMaps;

using AutoFixture;
using CommunityToolkit.Maui;
using Microsoft.Maui.Platform;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>().UseMauiMaps()
			   .UseMauiCommunityToolkit()
			   .UseMauiCommunityToolkitMaps("");
		builder.RegisterAppServices()
			.RegisterViewModels()
			.RegisterViews();
		builder.ConfigureMauiHandlers(handlers =>
		{
			handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
		});

		return builder.Build();
	}

	public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<IFixture, Fixture>();
		return mauiAppBuilder;
	}

	public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();

		return mauiAppBuilder;
	}

	public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<MainPage>();

		return mauiAppBuilder;
	}
}