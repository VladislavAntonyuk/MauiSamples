namespace IPTVPlayer.Views;

using ViewModels;

public partial class ChannelDetailPage : ContentPage
{
    public ChannelDetailPage(ChannelDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
	    MediaPlayer.Stop();
	    return base.OnBackButtonPressed();
    }
}
