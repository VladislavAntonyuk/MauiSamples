namespace MauiAuth
{
    public static class Constants
    {
        public static readonly string ClientId = System.Guid.Empty.ToString(); // "YOUR_CLIENT_ID_HERE"
        public static readonly string[] Scopes = new string[] { "openid", "offline_access" };
    }

}
