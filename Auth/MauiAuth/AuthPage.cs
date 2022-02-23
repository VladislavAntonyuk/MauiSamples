using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthServices;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Identity.Client;
using Button = Microsoft.Maui.Controls.Button;

namespace MauiAuth;

public abstract class AuthPage : ContentPage
{
    protected readonly IAuthService authService;
    private readonly Button loginButton;
    private readonly Button logoutButton;

    protected AuthPage(IAuthService authService)
    {
        this.authService = authService;
        loginButton = new Button
        {
            Text = "Login",
            Command = new Command(async () => await OnLoginClicked())
        };
        logoutButton = new Button
        {
            Text = "Logout",
            IsVisible = false,
            Command = new Command(async () => await OnLogoutClicked())
        };
        Content = new VerticalStackLayout
        {
            Children =
            {

                loginButton,
                logoutButton
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var result = await authService.AcquireTokenSilent(CancellationToken.None);
        await GetResult(result);
    }

    protected async Task GetResult(AuthenticationResult? result)
    {
        var token = result?.IdToken;
        if (token != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(token);
            if (data != null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Name: {data.Claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value}");
                await Toast.Make(stringBuilder.ToString()).Show();
                loginButton.IsVisible = false;
                logoutButton.IsVisible = true;
            }
        }
    }

    private async Task OnLoginClicked()
    {
        try
        {
            var result = await authService.SignInInteractively(CancellationToken.None);
            await GetResult(result);
        }
        catch (MsalClientException ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async Task OnLogoutClicked()
    {
        await authService.LogoutAsync(CancellationToken.None);
        loginButton.IsVisible = true;
        logoutButton.IsVisible = false;
    }
}