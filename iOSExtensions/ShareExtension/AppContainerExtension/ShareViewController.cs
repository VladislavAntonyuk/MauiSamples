using Foundation;
using Social;

namespace AppContainerExtension;

public partial class ShareViewController : SLComposeServiceViewController
{
	protected ShareViewController(IntPtr handle) : base(handle)
	{
		// Note: this .ctor should not contain any initialization logic.
	}

	public override void DidReceiveMemoryWarning()
	{
		// Releases the view if it doesn't have a superview.
		base.DidReceiveMemoryWarning();

		// Release any cached data, images, etc that aren't in use.
	}

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		// Do any additional setup after loading the view.
	}

	public override bool IsContentValid()
	{
		// Do validation of contentText and/or NSExtensionContext attachments here
		return true;
	}

	public override void DidSelectPost()
	{
		// This is called after the user selects Post. Do the upload of contentText and/or NSExtensionContext attachments.

		// Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
		ExtensionContext?.CompleteRequest(Array.Empty<NSExtensionItem>(), null);
	}

	public override SLComposeSheetConfigurationItem[] GetConfigurationItems()
	{
		// To add configuration options via table cells at the bottom of the sheet, return an array of SLComposeSheetConfigurationItem here.
		return Array.Empty<SLComposeSheetConfigurationItem>();
	}
}