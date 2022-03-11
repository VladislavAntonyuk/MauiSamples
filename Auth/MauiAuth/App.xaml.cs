using Application = Microsoft.Maui.Controls.Application;

namespace MauiAuth;

public partial class App : Application
{		
    public App(MainPage mainPage)
    {
        InitializeComponent();

        MainPage = new NavigationPage(mainPage);
    }
}