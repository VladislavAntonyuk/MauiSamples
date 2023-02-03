namespace MauiAuth;
using AuthServices;

public class AzureADPage : AuthPage
{
	public AzureADPage(AuthService authService) : base(authService)
	{
		Title = "Azure AD Login";
	}
}