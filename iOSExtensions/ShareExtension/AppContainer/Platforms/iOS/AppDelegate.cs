using Foundation;

namespace AppContainer;

using UIKit;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
	private const string CustomUrlScheme = "mauiapp";
	private const string ShareUrlHost = "openFromShare";
	private const string AppGroupIdentifier = "group.com.yourcompany.mauiapp";
	private const string SharedImageKey = "shared_image";

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
	{
		//check if image for our share extension
		if (url.Scheme == CustomUrlScheme && url.Host == ShareUrlHost)
		{
			try
			{
				var defaults = new NSUserDefaults(AppGroupIdentifier, NSUserDefaultsType.SuiteName);

				if (defaults.ValueForKey(new NSString(SharedImageKey)) is NSData {Length: > 0} data)
				{
					byte[] byteArray = data.ToArray();

					(Microsoft.Maui.Controls.Application.Current as App)?.LoadImage(byteArray);
					// Clean up shared data after successful read
					defaults.RemoveObject(SharedImageKey);
					defaults.Synchronize();
					return true;
				}

				Console.WriteLine("[iOS] No shared image data found or data is empty");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[iOS] Failed to load shared image: {ex.Message}");
				return false;
			}
		}

		return base.OpenUrl(app, url, options);
	}
}