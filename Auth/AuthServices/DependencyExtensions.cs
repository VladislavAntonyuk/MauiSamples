using Microsoft.Identity.Client;

namespace AuthServices;

public static class DependencyExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        var clientApplicationBuilder = PublicClientApplicationBuilder.Create(Constants.ClientId)
#if ANDROID
            .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif
            .WithRedirectUri($"msal{Constants.ClientId}://auth");

        services.AddSingleton<AuthServices.AuthService>(new AuthService(clientApplicationBuilder.Build()));

        services.AddSingleton<AuthServices.AuthServiceB2C>(new AuthServiceB2C(
            clientApplicationBuilder
                .WithIosKeychainSecurityGroup(Constants.IosKeychainSecurityGroups)
                .WithB2CAuthority(Constants.AuthoritySignIn)
                .Build()));
        
    }
}