namespace MauiSqliteBlazor;
using SqliteRepository;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddSingleton(new AccountRepository("accounts-blazor.db"));
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		return builder.Build();
	}
}