using Microsoft.Identity.Client;

namespace AuthServices;

public class AuthServiceB2C : BaseAuthService
{
    public AuthServiceB2C()
    {
        authenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
            .WithIosKeychainSecurityGroup(Constants.IosKeychainSecurityGroups)
            .WithB2CAuthority(Constants.AuthoritySignIn)
            .WithRedirectUri($"msal{Constants.ClientId}://auth")
#if ANDROID
            .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif
            .Build();
    }
}