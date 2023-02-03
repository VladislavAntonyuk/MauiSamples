﻿namespace MauiAuthBlazor;
using Foundation;
using Microsoft.Identity.Client;
using UIKit;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
	{
		AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
		return base.OpenUrl(app, url, options);
	}
}