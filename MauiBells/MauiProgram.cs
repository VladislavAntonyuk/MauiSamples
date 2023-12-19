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
				   handlers.AddHandler<CalendarView, CalendarHandler>();
			   });

		return builder.Build();
	}
}