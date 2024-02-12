namespace AuthServices;

using Microsoft.Identity.Client;

public class AuthServiceB2C(IPublicClientApplication publicClientApplication) : BaseAuthService(publicClientApplication);