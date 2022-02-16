using Microsoft.Identity.Client;

namespace AuthServices;

public class AuthService
{
    private readonly IPublicClientApplication authenticationClient;
    public AuthService()
    {
        authenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
            //.WithB2CAuthority(Constants.AuthoritySignIn) // uncomment to support B2C
            .WithRedirectUri($"msal{Constants.ClientId}://auth")
#if ANDROID
            .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif
            .Build();
    }

    public Task<AuthenticationResult?> LoginAsync(CancellationToken cancellationToken)
    {
        return authenticationClient
                   .AcquireTokenInteractive(Constants.Scopes)
                   .WithPrompt(Prompt.ForceLogin)
                .ExecuteAsync(cancellationToken);
    }
}