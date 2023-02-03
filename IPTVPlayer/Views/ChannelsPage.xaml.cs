namespace IPTVPlayer.Views;

using ViewModels;

public partial class ChannelsPage : ContentPage
{
	private readonly ChannelsViewModel viewModel;

	public ChannelsPage(ChannelsViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
		BindingContext = viewModel;
	}


	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		await viewModel.LoadDataAsync();
	}
}