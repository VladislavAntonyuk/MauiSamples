using Microsoft.Identity.Client;

namespace MauiAuth;

public class AuthService
{
    private readonly IPublicClientApplication authenticationClient;
    public AuthService()
    {
        authenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
            //.WithB2CAuthority(Constants.AuthoritySignIn) // uncomment to support B2C
            .WithRedirectUri($"msal{Constants.ClientId}://auth")
            .Build();
    }

    public Task<AuthenticationResult?> LoginAsync(CancellationToken cancellationToken)
    {
        return authenticationClient
                   .AcquireTokenInteractive(Constants.Scopes)
                   .WithPrompt(Prompt.ForceLogin)
#if ANDROID
                .WithParentActivityOrWindow(Platform.CurrentActivity)
#endif
                .ExecuteAsync(cancellationToken);
    }
}