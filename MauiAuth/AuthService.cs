using Microsoft.Identity.Client;
using System.Threading;
using System.Threading.Tasks;

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

    public async Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken)
    {
        AuthenticationResult result;
        try
        {
            result = await authenticationClient
                .AcquireTokenInteractive(Constants.Scopes)
                .WithPrompt(Prompt.ForceLogin)
#if ANDROID
                .WithParentActivityOrWindow(Microsoft.Maui.Essentials.Platform.CurrentActivity)
#endif
                .ExecuteAsync(cancellationToken);
            return result;
        }
        catch (MsalClientException)
        {
            return null;
        }
    }
}