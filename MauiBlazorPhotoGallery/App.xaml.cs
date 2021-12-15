using Application = Microsoft.Maui.Controls.Application;

namespace MauiBlazorPhotoGallery;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}
