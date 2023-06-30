#if WINDOWS || MACCATALYST
namespace MauiPaint;

using Microsoft.Maui.Platform;

public static class DragDropExtensions
{
	public static void RegisterDrag(this IElement element, IMauiContext? mauiContext, Func<CancellationToken, Task<Stream>> content)
	{
		ArgumentNullException.ThrowIfNull(mauiContext);
		var view = element.ToPlatform(mauiContext);
		DragDropHelper.RegisterDrag(view, content);
	}

	public static void UnRegisterDrag(this IElement element, IMauiContext? mauiContext)
	{
		ArgumentNullException.ThrowIfNull(mauiContext);
		var view = element.ToPlatform(mauiContext);
		DragDropHelper.UnRegisterDrag(view);
	}

	public static void RegisterDrop(this IElement element, IMauiContext? mauiContext, Func<Stream, Task>? content)
	{
		ArgumentNullException.ThrowIfNull(mauiContext);
		var view = element.ToPlatform(mauiContext);
		DragDropHelper.RegisterDrop(view, content);
	}

	public static void UnRegisterDrop(this IElement element, IMauiContext? mauiContext)
	{
		ArgumentNullException.ThrowIfNull(mauiContext);
		var view = element.ToPlatform(mauiContext);
		DragDropHelper.UnRegisterDrop(view);
	}
}
#endif