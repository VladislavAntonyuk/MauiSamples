using Microsoft.Extensions.Logging;

namespace MauiDraggableView;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			   .ConfigureMauiHandlers(h =>
			   {
				   h.AddHandler<DraggableView, DraggableViewHandler>();
			   });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}