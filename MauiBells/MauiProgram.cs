namespace MauiBells;

using Calendar;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			   .ConfigureMauiHandlers(handlers =>
			   {
#if ANDROID
				   handlers.AddHandler<CalendarView, CalendarMaterialHandler>();
#else
				   handlers.AddHandler<CalendarView, CalendarHandler>();
#endif
			   });

		return builder.Build();
	}
}