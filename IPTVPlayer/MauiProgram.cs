namespace IPTVPlayer;

using CommunityToolkit.Maui;
using Services;
using ViewModels;
using Views;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
#if WINDOWS
		NativeMethods.SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED);
#endif
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

#if WINDOWS
public enum EXECUTION_STATE : uint
{
    ES_AWAYMODE_REQUIRED = 0x00000040,
    ES_CONTINUOUS = 0x80000000,
    ES_DISPLAY_REQUIRED = 0x00000002,
}

internal class NativeMethods
{
    [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
    public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
}
#endif