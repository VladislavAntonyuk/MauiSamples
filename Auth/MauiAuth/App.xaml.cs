using Application = Microsoft.Maui.Controls.Application;

namespace MauiAuth;

public partial class App : Application
{		
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new MainPage());
    }
}