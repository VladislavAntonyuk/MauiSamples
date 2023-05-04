using Microsoft.Extensions.Logging;

namespace MauiCursor;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

public enum CursorIcon
{
	Wait,
	Hand,
	Arrow,
	IBeam,
	Cross,
	SizeAll
}