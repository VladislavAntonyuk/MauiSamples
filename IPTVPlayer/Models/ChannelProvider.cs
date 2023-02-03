namespace IPTVPlayer.Models;

public class ChannelProvider
{
	public ChannelProvider(string name, string? userAgent = null)
	{
		Name = name;
		UserAgent = userAgent;
	}

	public string Name { get; }
	public string? UserAgent { get; }
}