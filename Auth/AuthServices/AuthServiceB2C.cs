namespace AuthServices;

using Microsoft.Identity.Client;

public class AuthServiceB2C : BaseAuthService
{
	public AuthServiceB2C(IPublicClientApplication publicClientApplication) : base(publicClientApplication)
	{
	}
}