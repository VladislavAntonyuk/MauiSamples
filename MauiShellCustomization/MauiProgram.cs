using Microsoft.Extensions.Logging;

namespace MauiShellCustomization;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			   .ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler<Shell, CustomShellHandler>();
				}); ;

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}