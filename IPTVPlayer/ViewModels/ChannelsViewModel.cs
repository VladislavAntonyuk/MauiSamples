namespace IPTVPlayer.ViewModels;

using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;
using Views;

public partial class ChannelsViewModel : BaseViewModel
{
	readonly IPlaylistGenerator playlistGenerator;
	IEnumerable<Channel> allItems;

	public ObservableCollection<Channel> Items { get; } = new();

	[ObservableProperty]
	private string? filter;

	public ChannelsViewModel(IPlaylistGenerator service)
	{
		playlistGenerator = service;
		allItems = new List<Channel>();
	}

	[RelayCommand]
	private void Search(TextChangedEventArgs eventArgs)
	{
		Filter = eventArgs.NewTextValue;
		FilterData(Filter);
	}

	[RelayCommand]
	public async Task LoadDataAsync()
	{
		allItems = await playlistGenerator.GetPlaylist();
		FilterData(Filter);
	}

	void FilterData(string? channelName)
	{
		var filtered = allItems.Where(x => x.Name != null && x.Name.Contains(channelName ?? string.Empty, StringComparison.InvariantCultureIgnoreCase));
		Items.Clear();
		foreach (var item in filtered)
		{
			Items.Add(item);
		}
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