namespace MauiMarkdown;

static class AccessExtensions
{
	public static void Call(this object o, string methodName, params object[] args)
	{
		var mi = o.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		mi?.Invoke(o, args);
	}
}