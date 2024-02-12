﻿namespace MauiShellCustomization;

using CoreAnimation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using UIKit;

sealed class CustomShellToolbarAppearanceTracker(IShellContext shellContext, IShellNavBarAppearanceTracker baseTracker)
	: IShellNavBarAppearanceTracker
{
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
		if (controller.View is not null && shellContext.Shell.CurrentPage is not null)
		{
			controller.View.BackgroundColor = shellContext.Shell.CurrentPage.BackgroundColor.ToPlatform();
		}
	}

	public void UpdateLayout(UINavigationController controller)
	{
		baseTracker.UpdateLayout(controller);
		var topSpace = controller.NavigationBar.Bounds.Height / 2;
		controller.NavigationBar.Frame = new CoreGraphics.CGRect(controller.NavigationBar.Frame.X + topSpace,
																 controller.NavigationBar.Frame.Y + topSpace,
																 controller.NavigationBar.Frame.Width - 2 * topSpace,
																 controller.NavigationBar.Frame.Height);

		const int cornerRadius = 30;
		var uIBezierPath = UIBezierPath.FromRoundedRect(controller.NavigationBar.Bounds, UIRectCorner.AllCorners,
														new CoreGraphics.CGSize(cornerRadius, cornerRadius));

		var cAShapeLayer = new CAShapeLayer
		{
			Frame = controller.NavigationBar.Bounds,
			Path = uIBezierPath.CGPath
		};
		controller.NavigationBar.Layer.Mask = cAShapeLayer;
	}

	public void SetHasShadow(UINavigationController controller, bool hasShadow)
	{
		baseTracker.SetHasShadow(controller, hasShadow);
	}
}