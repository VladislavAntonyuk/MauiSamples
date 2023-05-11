using CoreAnimation;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace MauiShellCustomization;

class RoundCornerBottomNavViewAppearanceTracker : ShellTabBarAppearanceTracker
{
	public override void SetAppearance(UITabBarController controller, ShellAppearance appearance)
	{
		var uIBezierPath = UIBezierPath.FromRoundedRect(controller.TabBar.Bounds, UIRectCorner.TopLeft | UIRectCorner.TopRight, new CoreGraphics.CGSize(30, 30));
		var cAShapeLayer = new CAShapeLayer
		{
			Frame = controller.TabBar.Bounds,
			Path = uIBezierPath.CGPath
		};
		controller.TabBar.Layer.Mask = cAShapeLayer;
	}
}