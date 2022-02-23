using AuthServices;

namespace MauiAuth;

public class AzureADPage : AuthPage
{
    public AzureADPage() : base(new AuthService())
    {
    }
}