using Microsoft.Identity.Client;

namespace AuthServices;

public class AuthServiceB2C : BaseAuthService
{
    public AuthServiceB2C(IPublicClientApplication publicClientApplication) : base(publicClientApplication)
    {
    }
}