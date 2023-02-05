namespace AuthServices;

public static class Constants
{
	public static readonly string ClientId = Guid.Empty.ToString(); // "YOUR_CLIENT_ID_HERE"
	public static readonly string[] Scopes = { "openid", "offline_access" };
	// The next code to add B2C
	public static readonly string TenantName = "YOUR_TENANT_NAME";
	public static readonly string TenantId = $"{TenantName}.onmicrosoft.com";
	public static readonly string SignInPolicy = "B2C_1_Client";
	public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
	public static readonly string AuthoritySignIn = $"{AuthorityBase}{SignInPolicy}";
	public static readonly string IosKeychainSecurityGroups = "com.microsoft.adalcache";
}