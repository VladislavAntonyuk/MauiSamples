using System;
using Microsoft.Extensions.DependencyInjection;
using Application = Microsoft.Maui.Controls.Application;

namespace MauiSqlite;
public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        MainPage = serviceProvider.GetRequiredService<MainPage>();
    }
}
