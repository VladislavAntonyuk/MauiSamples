namespace MauiPaint;

using Foundation;
using MobileCoreServices;
using UIKit;

public class DragDropHelper
{
	public static void RegisterDragDrop(UIView view)
	{
		var dropInteraction = new UIDropInteraction(new DropInteractionDelegate());
		view.AddInteraction(dropInteraction);
	}
}

class DropInteractionDelegate : UIDropInteractionDelegate
{
	public override bool CanHandleSession(UIDropInteraction interaction, IUIDropSession session)
	{
		var csTypeIdentifier = UniformTypeIdentifiers.UTType.CreateFromExtension("cs")?.Identifier;
		var acceptedTypeIdentifiers = new [] { csTypeIdentifier };
		return session.HasConformingItems(acceptedTypeIdentifiers!);
		//return session.HasConformingItems(new string[] { UniformTypeIdentifiers.UTTypes.Text.Identifier });
	}

	public override void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
	{
		if (session.HasConformingItems(new string[]
		    {
			    UniformTypeIdentifiers.UTTypes.Text.Identifier
			}))
		{
			session.LoadObjects<NSString>((items) => {
				if (items.Length > 0 && items[0] is NSString text)
				{
					// Handle dropped text
					Console.WriteLine(text.ToString());
				}
			});
		}

		session.LoadObjects<NSUrl>((items) => {
			if (items.Length > 0 && items[0] is NSUrl text)
			{
				// Handle dropped text
				Console.WriteLine(text.ToString());
			}
		});


	}
}