namespace MauiPaint;

using CoreFoundation;
using Foundation;
using MobileCoreServices;
using UIKit;

public static class DragDropHelper
{
	public static void RegisterDragDrop(UIView view, Func<Stream, Task>? content)
	{
		var dropInteraction = new UIDropInteraction(new DropInteractionDelegate()
		{
			Content = content
		});
		view.AddInteraction(dropInteraction);
	}

	public static void UnRegisterDragDrop(UIView view)
	{
		var dropInteractions = view.Interactions.OfType<UIDropInteraction>();
		foreach (var interaction in dropInteractions)
		{
			view.RemoveInteraction(interaction);
		}
	}
}

class DropInteractionDelegate : UIDropInteractionDelegate
{
	public Func<Stream, Task>? Content { get; init; }

	public override UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
	{
		return new UIDropProposal(UIDropOperation.Copy);
	}

	public override void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
	{
		foreach (var item in session.Items)
		{
			item.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Json.Identifier, null, async (data, error) =>
			{
				if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
				{
					if (Content is not null)
					{
						var bytes = await File.ReadAllBytesAsync(nsData.Path);
						await Content.Invoke(new MemoryStream(bytes));
					}
				}
			});
		}
	}
}