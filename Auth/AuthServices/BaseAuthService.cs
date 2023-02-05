namespace AuthServices;

using Microsoft.Identity.Client;

public abstract class BaseAuthService : IAuthService
{
	private readonly IPublicClientApplication authenticationClient;

	protected BaseAuthService(IPublicClientApplication authenticationClient)
	{
		this.authenticationClient = authenticationClient;
	}

	public Task<AuthenticationResult?> SignInInteractively(CancellationToken cancellationToken)
	{
		return authenticationClient
			   .AcquireTokenInteractive(Constants.Scopes)
#if WINDOWS
				.WithUseEmbeddedWebView(false)				
#endif
			   .ExecuteAsync(cancellationToken);
	}

	public async Task<AuthenticationResult?> AcquireTokenSilent(CancellationToken cancellationToken)
	{
		try
		{
			var accounts = await authenticationClient.GetAccountsAsync(Constants.SignInPolicy);
			var firstAccount = accounts.FirstOrDefault();
			if (firstAccount is null)
			{
				return null;
			}

			return await authenticationClient.AcquireTokenSilent(Constants.Scopes, firstAccount)
											 .ExecuteAsync(cancellationToken);
		}
		catch (MsalUiRequiredException)
		{
			return null;
		}
	}

	public async Task LogoutAsync(CancellationToken cancellationToken)
	{
		var accounts = await authenticationClient.GetAccountsAsync();
		foreach (var account in accounts)
		{
			await authenticationClient.RemoveAsync(account);
		}
	}
}