using MobileCoreServices;
using Social;
using UniformTypeIdentifiers;

namespace AppContainerExtension;

public partial class ShareViewController : SLComposeServiceViewController
{
	private const string AppGroupIdentifier = "group.com.yourcompany.mauiapp";
	private const string SharedImageKey = "shared_image";
	private const int MaxAttemptsToFindUiApplication = 100;

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

	public override async void DidSelectPost()
	{
		try
		{
			await ExportImageToMainApp();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
		finally
		{
			CompleteExtension();
		}
	}

	private async Task ExportImageToMainApp()
	{
		// This is called after the user selects Post. Do the upload of contentText and/or NSExtensionContext attachments.
		// Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.

		var tcs = new TaskCompletionSource<bool>();

		var extensionItem = ExtensionContext?.InputItems[0];
		var attachments = extensionItem?.Attachments;

		if (attachments is null)
		{
			return;
		}

		foreach (var itemProvider in attachments)
		{
			if (itemProvider.HasItemConformingTo(UTTypes.Image.ToString()))
			{
				itemProvider.LoadItem(UTTypes.Image.ToString(), null, (nsObject, _) =>
				{
					NSData? data = null;

					if (nsObject is NSUrl url) //from photos
					{
						data = NSData.FromUrl(url);
					}
					else if (nsObject is UIImage uiImage) //from screenshot editor
					{
						data = uiImage.AsPNG();
					}
					else if (nsObject is NSData {Length: > 0} nsData) //from screenshot editor for iOS 26+
					{
						data = nsData;
					}

					if (data is null)
					{
						tcs.TrySetResult(false);
					}

					var userDefaults =
						new NSUserDefaults(AppGroupIdentifier, NSUserDefaultsType.SuiteName);
					userDefaults.SetValueForKey(data!, new NSString(SharedImageKey));
					userDefaults.Synchronize();

					tcs.TrySetResult(true);
				});
			}
		}

		var res = await tcs.Task;

		if (!res)
		{
			CompleteExtension();
			return;
		}

		var schemeUrl = new NSUrl("mauiapp://openFromShare");
		OpenUrlInBrowser(schemeUrl);
	}


	private void OpenUrlInBrowser(NSUrl url)
	{
		UIResponder responder = this;

		int count = 0;
		//From the logs, should find UIApplication within max 10 iterations,
		//however, this is to make sure we exit, if it tries to find infinitely
		while (count < MaxAttemptsToFindUiApplication)
		{
			if (responder is UIApplication application)
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(18, 0))
				{
					application.OpenUrl(url, new UIApplicationOpenUrlOptions(), null);

					CompleteExtension();

					return;
				}

				//for ios < 18
				var sel = new ObjCRuntime.Selector("openURL:");
				application.PerformSelector(sel, url, 0f);

				CompleteExtension();

				return;
			}

			responder = responder.NextResponder;
			count++;
		}
	}

	private void CompleteExtension()
	{
		ExtensionContext?.CompleteRequest([], null);
	}

	public override SLComposeSheetConfigurationItem[] GetConfigurationItems()
	{
		// To add configuration options via table cells at the bottom of the sheet, return an array of SLComposeSheetConfigurationItem here.
		return Array.Empty<SLComposeSheetConfigurationItem>();
	}
}