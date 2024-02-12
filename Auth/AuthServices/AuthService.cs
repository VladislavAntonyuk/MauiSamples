namespace AuthServices;

using Microsoft.Identity.Client;

public class AuthService(IPublicClientApplication publicClientApplication) : BaseAuthService(publicClientApplication);