# .NET MAUI Push Notifications using Azure Notification Hub

[![Buy Me A Coffee](https://ik.imagekit.io/VladislavAntonyuk/vladislavantonyuk/misc/bmc-button.png)](https://www.buymeacoffee.com/vlad.antonyuk)

Article: https://vladislavantonyuk.github.io/articles/.NET-MAUI-Push-Notifications-using-Azure-Notification-Hub.-Part-2.-Setup-.NET-MAUI/

### Changes needed to make receiving notifications work on Android
- `Platforms\Android\google-services.json` - overwrite with your own
- `MauiNotifications.csproj` - replace `<ApplicationId>` with your own, likely something like `com.CompanyName.MauiNotifications`
- `MauiProgram.cs`
  - `YOUR CONNECTION STRING` - replace with your Azure Notification Hub connection string from "DefaultListenSharedAccessSignature".
	It should look something like this: `Endpoint=sb://<namespace>.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=<shared access key>`
  - `YOUR HUB NAME` - replace with your Azure Notification Hub name (just the hub name, not the namespace)

[![Stand With Ukraine](https://img.shields.io/badge/made_in-ukraine-ffd700.svg?labelColor=0057b7)](https://stand-with-ukraine.pp.ua)