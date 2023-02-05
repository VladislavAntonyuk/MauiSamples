namespace IPTVPlayer.Views;

using ViewModels;

public partial class ChannelDetailPage : ContentPage
{
	public ChannelDetailPage(ChannelDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnDisappearing()
	{
		MediaPlayer.Stop();
		base.OnDisappearing();
	}
}