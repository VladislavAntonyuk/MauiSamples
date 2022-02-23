namespace MauiAuth;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    async void AzureADPageClicked(object? sender, EventArgs args)
    {
        await Navigation.PushAsync(new AzureADPage());
    }

    async void AzureB2CPageClicked(object? sender, EventArgs args)
    {
        await Navigation.PushAsync(new AzureB2C());
    }
    
}