namespace IPTVPlayer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using Models;

public partial class ChannelDetailViewModel : BaseViewModel, IQueryAttributable
{
	[ObservableProperty]
	public partial Channel Item { get; set; }

	public ChannelDetailViewModel()
	{
		Item = new();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		Item = (Channel)query["Item"];
	}
}