# .NET Multi-platform App UI (.NET MAUI)

`.NET MAUI` is a cross-platform framework for creating mobile and desktop apps with C# and XAML. Using `.NET MAUI`, you can develop apps that can run on *Android*, *iOS*, *iPadOS*, *macOS*, and *Windows* from a single shared codebase. <span style="background:yellow"> <3 :-) :heart: </span>

## Getting Started

* __[.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui)__
* __[.NET MAUI Samples](https://github.com/dotnet/maui-samples)__

### MauiProgram.cs

```csharp
public static MauiApp CreateMauiApp()
{
	var builder = MauiApp.CreateBuilder();
	builder.UseMauiApp<App>();
	return builder.Build();
}
```

Learn more about __[.NET MAUI](https://dot.net/maui)__.
