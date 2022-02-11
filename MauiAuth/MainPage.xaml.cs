using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Identity.Client;

namespace MauiAuth;

public partial class MainPage : ContentPage
{
    private readonly AuthService authService;
    public MainPage()
    {
        InitializeComponent();
        authService = new AuthService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        AuthenticationResult? result = null;
        try
        {
            result = await authService.LoginAsync(CancellationToken.None);
        }
        catch (MsalClientException ex)
        {
            await Toast.Make(ex.Message).Show();
        }

        var token = result?.IdToken;
        if (token != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(token);
            if (data != null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Name: {data.Claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value}");
                LoginResultLabel.Text = stringBuilder.ToString();
            }
        }
    }
}