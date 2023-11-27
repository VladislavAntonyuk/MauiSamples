namespace IPTVPlayer.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

internal class PlaylistGenerator : IPlaylistGenerator
{
	public async Task<IEnumerable<Channel>> GetPlaylist(string source)
	{
		var playlist = new List<Channel>();
		var m3u = await m3uParser.M3U.ParseFromUrlAsync(source);
		playlist.AddRange(m3u.Medias.Select(x => new Channel()
		{
			Name = x.Title.RawTitle,
			Url = x.MediaFile,
			Image = x.Attributes.TvgLogo,
			Provider = new ChannelProvider(x.Attributes.GroupTitle)
		}));
		return playlist.OrderBy(x => x.Name);
	}
}