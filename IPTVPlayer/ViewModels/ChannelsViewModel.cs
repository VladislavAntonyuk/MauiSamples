namespace IPTVPlayer.ViewModels;

using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;
using Views;

public partial class ChannelsViewModel(IPlaylistGenerator service) : BaseViewModel
{
	IEnumerable<Channel> allItems = new List<Channel>();

	public ObservableCollection<Channel> Items { get; } = new();
	public ObservableCollection<string> Sources { get; } = new()
	{
		"https://iptv.org.ua/iptv/ua.m3u",
		"https://mater.com.ua/ip/ua.m3u",
		"https://tva.org.ua/ip/u/iptv_ukr.m3u",
		"https://tva.org.ua/ip/sam/avto-full.m3u"
	};

	[ObservableProperty]
	public partial string? Filter { get; set; }

	[ObservableProperty]
	public partial int SelectedIndex { get; set; }

	[RelayCommand]
	private void Search(TextChangedEventArgs eventArgs)
	{
		Filter = eventArgs.NewTextValue;
		FilterData(Filter);
	}

	[RelayCommand]
	public async Task LoadData()
	{
		var source = Sources[SelectedIndex];
		allItems = await service.GetPlaylist(source);
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