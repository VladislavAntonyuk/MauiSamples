# .NET MAUI TabView

[![Buy Me A Coffee](https://ik.imagekit.io/VladislavAntonyuk/vladislavantonyuk/misc/bmc-button.png)](https://www.buymeacoffee.com/vlad.antonyuk)

Article: https://vladislavantonyuk.github.io/articles/Mastering-Composite-Controls-in-.NET-MAUI:-Building-a-TabView-from-Scratch

## Features

- Custom TabView control for .NET MAUI
- Support for multiple TabView instances on the same page
- Handles shared Tab objects across multiple TabView instances
- Compatible with Android, iOS, macOS, and Windows

## Multiple TabView Support

You can now use multiple TabView instances on the same page, even when they share the same Tab objects:

```xml
<mauiTabView:TabView Tabs="{Binding Tabs1}"/>
<mauiTabView:TabView Tabs="{Binding Tabs2}"/>
```

The TabView automatically handles the case where Tab objects are shared between multiple TabView instances by removing content from its previous parent before adding it to the new one.

## Images

![.NET MAUI TabView](https://ik.imagekit.io/VladislavAntonyuk/vladislavantonyuk/articles/48/48.png)

[![Stand With Ukraine](https://img.shields.io/badge/made_in-ukraine-ffd700.svg?labelColor=0057b7)](https://stand-with-ukraine.pp.ua)