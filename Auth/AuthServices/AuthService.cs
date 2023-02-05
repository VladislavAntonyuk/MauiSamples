namespace AuthServices;

using Microsoft.Identity.Client;

public class AuthService : BaseAuthService
{
	public AuthService(IPublicClientApplication publicClientApplication) : base(publicClientApplication)
	{
	}
}