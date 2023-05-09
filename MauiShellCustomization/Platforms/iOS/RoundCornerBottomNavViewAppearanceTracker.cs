using CoreAnimation;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace MauiShellCustomization;

class RoundCornerBottomNavViewAppearanceTracker : ShellTabBarAppearanceTracker
{
    public override void SetAppearance(UITabBarController controller, ShellAppearance appearance)
    {
        UIBezierPath uIBezierPath = UIBezierPath.FromRoundedRect(controller.TabBar.Bounds, UIRectCorner.TopLeft | UIRectCorner.TopRight, new CoreGraphics.CGSize(30, 30));
        CAShapeLayer cAShapeLayer = new CAShapeLayer();
        cAShapeLayer.Frame = controller.TabBar.Bounds;
        cAShapeLayer.Path = uIBezierPath.CGPath;
        controller.TabBar.Layer.Mask = cAShapeLayer;
    }
}