namespace Sample
{
	using Calendar;
	using MauiBells.Calendar;

	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				})
				.ConfigureMauiHandlers(x =>
				{
					x.AddHandler<CalendarView, CalendarMaterialHandler>();
				});

			return builder.Build();
		}
	}
}
