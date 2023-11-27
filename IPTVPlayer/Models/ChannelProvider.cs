namespace IPTVPlayer.Models;

public class ChannelProvider(string name, string? userAgent = null)
{
	public string Name { get; } = name;
	public string? UserAgent { get; } = userAgent;
}