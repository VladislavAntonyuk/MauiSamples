namespace Microsoft.Identity.Client;

using Foundation;

/// <summary>
/// Static class that consumes the response from the Authentication flow and continues token acquisition. This class should be called in ApplicationDelegate whenever app loads/reloads.
/// </summary>
public static class AuthenticationContinuationHelper
{

	public static bool ContinueAuthentication(string url)
	{
		//possible false
		return true;
	}

	/// <summary>
	/// Sets response for continuing authentication flow. This function will return true if the response was meant for MSAL, else it will return false.
	/// </summary>
	/// <param name="url">url used to invoke the application</param>
	public static bool SetAuthenticationContinuationEventArgs(NSUrl url)
	{
		return ContinueAuthentication(url.AbsoluteString);
	}
}