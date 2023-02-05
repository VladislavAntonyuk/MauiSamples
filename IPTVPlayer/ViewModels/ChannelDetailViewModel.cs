namespace IPTVPlayer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using Models;

[QueryProperty(nameof(Item), "Item")]
public partial class ChannelDetailViewModel : BaseViewModel
{
	[ObservableProperty]
	Channel item = new();
}