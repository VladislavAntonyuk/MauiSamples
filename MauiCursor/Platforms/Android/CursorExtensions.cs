namespace MauiCursor;

using System;
using Android.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Application = Android.App.Application;

public static class CursorExtensions
{
	public static void SetCustomCursor(this VisualElement visualElement, CursorIcon cursor)
	{
		var mauiContext = visualElement.Handler?.MauiContext ?? throw new NullReferenceException("MauiContext is null");
		var view = visualElement.ToPlatform(mauiContext);
		if (OperatingSystem.IsAndroidVersionAtLeast(24))
		{
			view.PointerIcon = PointerIcon.GetSystemIcon(Application.Context, GetCursor(cursor));
		}
	}

	static PointerIconType GetCursor(CursorIcon cursor)
	{
		return cursor switch
		{
			CursorIcon.Hand => PointerIconType.Hand,
			CursorIcon.IBeam => PointerIconType.AllScroll,
			CursorIcon.Cross => PointerIconType.Crosshair,
			CursorIcon.Arrow => PointerIconType.Arrow,
			CursorIcon.SizeAll => PointerIconType.TopRightDiagonalDoubleArrow,
			CursorIcon.Wait => PointerIconType.Wait,
			_ => PointerIconType.Default,
		};
	}
}