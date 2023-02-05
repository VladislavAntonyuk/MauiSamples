namespace IPTVPlayer.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

public interface IPlaylistGenerator
{
	Task<IEnumerable<Channel>> GetPlaylist();
}