namespace MauiBank;
using Foundation;
using UIKit;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override void OnResignActivation(UIApplication application)
	{
		var keyWindow = GetKeyWindow(application);
		if (keyWindow is null)
		{
			return;
		}

		var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark);
		var blurEffectView = new UIVisualEffectView(blurEffect)
		{
			Frame = keyWindow.Subviews[0].Bounds,
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions,
			Tag = new IntPtr(12)
		};
		keyWindow.AddSubview(blurEffectView);
		base.OnResignActivation(application);
	}

	public override void OnActivated(UIApplication uiApplication)
	{
		var sub = GetKeyWindow(uiApplication);
		if (sub == null)
		{
			return;
		}

		foreach (var vv in sub.Subviews)
		{
			if (vv.Tag == new IntPtr(12))
			{
				vv.RemoveFromSuperview();
			}
		}

		base.OnActivated(uiApplication);
	}

	private static UIWindow? GetKeyWindow(UIApplication uiApplication)
	{
		return uiApplication.ConnectedScenes.ToArray()
							.Select(x => x as UIWindowScene)
							.FirstOrDefault()?
							.Windows.FirstOrDefault(x => x.IsKeyWindow);
	}
}