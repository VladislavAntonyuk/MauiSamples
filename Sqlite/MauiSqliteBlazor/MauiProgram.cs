namespace MauiSqliteBlazor;
using Microsoft.AspNetCore.Components.WebView.Maui;
using SqliteRepository;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>();

        builder.Services.AddBlazorWebView();
        builder.Services.AddSingleton(new AccountRepository("accounts-blazor.db"));

        return builder.Build();
    }
}
