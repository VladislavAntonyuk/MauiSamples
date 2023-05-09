using Microsoft.Extensions.Logging;

namespace MauiShellCustomization;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
		       .ConfigureMauiHandlers(h =>
				{
					h.AddHandler(typeof(Shell), typeof(RoundCornerTabBarShellHandler));
				}); ;

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}