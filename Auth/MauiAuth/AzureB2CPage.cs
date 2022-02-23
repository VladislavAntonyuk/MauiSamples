using AuthServices;

namespace MauiAuth;

public class AzureB2C : AuthPage
{
    public AzureB2C() : base(new AuthServiceB2C())
    {
    }
}