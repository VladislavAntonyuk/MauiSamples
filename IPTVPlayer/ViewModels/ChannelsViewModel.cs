namespace IPTVPlayer.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;
using Views;

public partial class ChannelsViewModel : BaseViewModel
{
	readonly IPlaylistGenerator playlistGenerator;

	[ObservableProperty]
	bool isRefreshing;

	[ObservableProperty]
	ObservableCollection<Channel> items = new();

	public ChannelsViewModel(IPlaylistGenerator service)
	{
		playlistGenerator = service;
	}

	[RelayCommand]
	private async Task OnRefreshing()
	{
		IsRefreshing = true;

		try
		{
			await LoadDataAsync();
		}
		finally
		{
			IsRefreshing = false;
		}
	}

	public async Task LoadDataAsync()
	{
		Items = new ObservableCollection<Channel>(await playlistGenerator.GetPlaylist());
	}

	[RelayCommand]
	private async Task GoToDetails(Channel item)
	{
		await Shell.Current.GoToAsync(nameof(ChannelDetailPage), true, new Dictionary<string, object>
		{
			{ "Item", item }
		});
	}
}