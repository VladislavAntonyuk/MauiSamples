namespace IPTVPlayer.Services;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Models;

internal class TvUaPlaylistGenerator : IPlaylistGenerator
{
	private readonly HttpClient httpClient;

	public TvUaPlaylistGenerator(HttpClient httpClient)
	{
		this.httpClient = httpClient;
	}

	public async Task<IEnumerable<Channel>> GetPlaylist()
	{
		var playlist = new List<Channel>();
		var response = await httpClient.GetStringAsync("https://tva.org.ua/ip/u/iptv_ukr.m3u");
		var data = response.Replace("#EXTINF:-1,", "").Replace("#EXTINF:-1 ,", "").Trim();
		var split = data.Split(new[] { '\n' });
		for (var i = 1; i < split.Length - 1; i += 2)
		{
			playlist.Add(new Channel
			{
				Name = split[i],
				Url = split[i + 1]
			});
		}

		return playlist.OrderBy(x => x.Name);
	}
}