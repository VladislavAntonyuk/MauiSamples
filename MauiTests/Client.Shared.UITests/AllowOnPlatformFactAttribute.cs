namespace Client.UITests;

using System.Runtime.InteropServices;

public sealed class AllowOnPlatformFactAttribute : FactAttribute
{
	public AllowOnPlatformFactAttribute(params string[] runtimePlatforms)
	{
		foreach (var runtimePlatformString in runtimePlatforms)
		{
			var runtimePlatform = RuntimePlatform.Parse(runtimePlatformString);
			if (RuntimeInformation.IsOSPlatform(runtimePlatform.Runtime) && AppiumSetup.Platform == runtimePlatform.Platform)
			{
				Skip = null;
				return;
			}

			Skip = $"Test cannot be executed only on the {runtimePlatformString} platform";
		}
	}
}
