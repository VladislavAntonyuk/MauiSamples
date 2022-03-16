using Microsoft.Identity.Client;

namespace AuthServices;

public class AuthService : BaseAuthService
{
    public AuthService(IPublicClientApplication publicClientApplication) : base(publicClientApplication)
    {
    }
}