using CommunityToolkit.Maui;

namespace MauiShellCustomization;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
			   .ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler<Shell, CustomShellHandler>();
				}); ;

		return builder.Build();
	}
}