namespace MauiBank;

using CommunityToolkit.Maui;
using ViewModels;
using Views;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FASolid");
				fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FABrands");
				fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FARegular");
			});

		builder.Services.AddSingleton<CardPage, CardPageViewModel>();
		builder.Services.AddSingleton<PinPage, PinPageViewModel>();
		builder.Services.AddSingletonWithShellRoute<ProfilePage, ProfilePageViewModel>("//home/profile");
		return builder.Build();
	}
}