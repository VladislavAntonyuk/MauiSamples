namespace IPTVPlayer;

using CommunityToolkit.Maui;
using Services;
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
			.UseMauiCommunityToolkitMediaElement();

		builder.Services.AddHttpClient();
		builder.Services.AddTransient<IPlaylistGenerator, TvUaPlaylistGenerator>();
		builder.Services.AddTransientWithShellRoute<ChannelDetailPage, ChannelDetailViewModel>(nameof(ChannelDetailPage));
		builder.Services.AddSingleton<ChannelsPage, ChannelsViewModel>();

		return builder.Build();
	}
}
