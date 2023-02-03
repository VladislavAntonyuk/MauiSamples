namespace AuthServices;
using Microsoft.Identity.Client;

public interface IAuthService
{
	Task<AuthenticationResult?> SignInInteractively(CancellationToken cancellationToken);
	Task<AuthenticationResult?> AcquireTokenSilent(CancellationToken cancellationToken);
	Task LogoutAsync(CancellationToken cancellationToken);
}