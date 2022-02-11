using Foundation;
using SQLitePCL;

namespace KanbanBoard;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        Batteries_V2.Init();
        return MauiProgram.CreateMauiApp();
    }
}
