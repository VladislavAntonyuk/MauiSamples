namespace MauiApplicationInsights;

using Microsoft.Extensions.Logging;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Logging.AddApplicationInsights(configuration =>
		{
			configuration.TelemetryInitializers.Add(new ApplicationInitializer());
			configuration.ConnectionString = "InstrumentationKey=c60a25d7-5618-4a3d-bcdd-0a6912f3e7ac;IngestionEndpoint=https://northeurope-2.in.applicationinsights.azure.com/;LiveEndpoint=https://northeurope.livediagnostics.monitor.azure.com/";

		}, options =>
		{
			options.IncludeScopes = true;
		});
		builder.Services.AddSingleton<MainPage>();

		return builder.Build();
	}
}