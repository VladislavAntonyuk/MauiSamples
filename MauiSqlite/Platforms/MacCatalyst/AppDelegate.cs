using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using SQLitePCL;

namespace MauiSqlite;
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        raw.SetProvider(new SQLite3Provider_sqlite3());
        return MauiProgram.CreateMauiApp();
    }
}
