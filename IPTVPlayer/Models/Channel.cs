namespace IPTVPlayer.Models;

using System.Collections.Generic;

public record Channel
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Url { get; set; }
	public string? Image { get; set; }
	public ChannelProvider? Provider { get; set; }
	public IEnumerable<EPG>? EPG { get; set; }
}