namespace MauiAuth;
using AuthServices;

public class AzureB2CPage : AuthPage
{
	public AzureB2CPage(AuthServiceB2C authServiceB2C) : base(authServiceB2C)
	{
		Title = "Azure AD B2C Login page";
	}
}