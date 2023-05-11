namespace MauiShellCustomization;

using CoreAnimation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

class RoundCornerShellToolbarAppearanceTracker : IShellNavBarAppearanceTracker
{
	private readonly IShellNavBarAppearanceTracker baseTracker;

	public RoundCornerShellToolbarAppearanceTracker(IShellNavBarAppearanceTracker baseTracker)
	{
		this.baseTracker = baseTracker;
	}

	public void Dispose()
	{
		baseTracker.Dispose();
	}

	public void ResetAppearance(UINavigationController controller)
	{
		baseTracker.ResetAppearance(controller);
	}

	public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
	{
		baseTracker.SetAppearance(controller, appearance);
		var uIBezierPath = UIBezierPath.FromRoundedRect(controller.NavigationBar.Bounds, UIRectCorner.TopLeft | UIRectCorner.TopRight, new CoreGraphics.CGSize(30, 30));
		var cAShapeLayer = new CAShapeLayer
		{
			Frame = controller.NavigationBar.Bounds,
			Path = uIBezierPath.CGPath
		};
		controller.NavigationBar.Layer.Mask = cAShapeLayer;
	}

	public void UpdateLayout(UINavigationController controller)
	{
		baseTracker.UpdateLayout(controller);
	}

	public void SetHasShadow(UINavigationController controller, bool hasShadow)
	{
		baseTracker.SetHasShadow(controller, hasShadow);
	}
}