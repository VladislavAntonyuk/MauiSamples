namespace MauiCursor;

using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.UI.Core;

public static class CursorExtensions
{
	public static void SetCustomCursor(this VisualElement visualElement, CursorIcon cursor, IMauiContext? mauiContext)
	{
		ArgumentNullException.ThrowIfNull(mauiContext);
		UIElement view = visualElement.ToPlatform(mauiContext);
		view.PointerEntered += ViewOnPointerEntered;
		view.PointerExited += ViewOnPointerExited;
		void ViewOnPointerExited(object sender, PointerRoutedEventArgs e)
		{
			view.ChangeCursor(InputCursor.CreateFromCoreCursor(new CoreCursor(GetCursor(CursorIcon.Arrow), 1)));
		}

		void ViewOnPointerEntered(object sender, PointerRoutedEventArgs e)
		{
			view.ChangeCursor(InputCursor.CreateFromCoreCursor(new CoreCursor(GetCursor(cursor), 1)));
		}
	}

	static void ChangeCursor(this UIElement uiElement, InputCursor cursor)
	{
		Type type = typeof(UIElement);
		type.InvokeMember("ProtectedCursor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, uiElement, new object[] { cursor });
	}

	static CoreCursorType GetCursor(CursorIcon cursor)
	{
		return cursor switch
		{
			CursorIcon.Hand => CoreCursorType.Hand,
			CursorIcon.IBeam => CoreCursorType.IBeam,
			CursorIcon.Cross => CoreCursorType.Cross,
			CursorIcon.Arrow => CoreCursorType.Arrow,
			CursorIcon.SizeAll => CoreCursorType.SizeAll,
			CursorIcon.Wait => CoreCursorType.Wait,
			_ => CoreCursorType.Arrow,
		};
	}
}