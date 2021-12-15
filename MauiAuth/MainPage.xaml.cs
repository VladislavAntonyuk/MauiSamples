using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Maui.Controls;

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
        var result = await authService.LoginAsync(CancellationToken.None);
        var token = result?.IdToken;
        if (token != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(token);
            var claims = data.Claims.ToList();
            if (data != null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Name: {data.Claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value}");
                LoginResultLabel.Text = stringBuilder.ToString();
            }
        }
    }
}