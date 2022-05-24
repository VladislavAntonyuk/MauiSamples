namespace AuthServices;

using Microsoft.Identity.Client;

public static class DependencyExtensions
{
	public static void RegisterServices(this IServiceCollection services)
	{
		var clientApplicationBuilder = PublicClientApplicationBuilder.Create(Constants.ClientId)
#if ANDROID
			.WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif
			.WithRedirectUri($"msal{Constants.ClientId}://auth");

		services.AddSingleton(new AuthService(clientApplicationBuilder.Build()));

		var b2cClientApplicationBuilder = PublicClientApplicationBuilder.Create(Constants.ClientId)
#if ANDROID
			.WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif
			.WithRedirectUri($"msal{Constants.ClientId}://auth");
		
		services.AddSingleton(new AuthServiceB2C(
			b2cClientApplicationBuilder
				.WithIosKeychainSecurityGroup(Constants.IosKeychainSecurityGroups)
				.WithB2CAuthority(Constants.AuthoritySignIn)
				.Build()));

	}
}