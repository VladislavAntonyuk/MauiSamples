namespace Client.UITests;

using System.Runtime.InteropServices;

public class RuntimePlatform
{
	public const string WindowsAndroid = "WINDOWS,Android";
	public const string OsxAndroid = "OSX,Android";
	public const string OsxIOs = "OSX,iOS";
	public const string OsxMacCatalyst = "OSX,MacCatalyst";
	public const string WindowsWindows = "WINDOWS,Windows";
	public const string OsxTizen = "OSX,Tizen";

	public static RuntimePlatform Parse(string runtimePlatform)
	{
		var data = runtimePlatform.Split(',');
		return new RuntimePlatform
		{
			Runtime = OSPlatform.Create(data[0]),
			Platform = data[1]
		};
	}

	public string? Platform { get; private init; }

	public OSPlatform Runtime { get; private init; }
}