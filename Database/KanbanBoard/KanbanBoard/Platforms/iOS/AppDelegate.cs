namespace KanbanBoard;
using Foundation;
using SQLitePCL;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp()
	{
		Batteries_V2.Init();
		return MauiProgram.CreateMauiApp();
	}
}