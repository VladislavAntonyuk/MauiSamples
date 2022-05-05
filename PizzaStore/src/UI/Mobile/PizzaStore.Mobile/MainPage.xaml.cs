namespace PizzaStore.Mobile;

using CommunityToolkit.Maui.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();
		BindingContext = mainViewModel;
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		ShowPopup();
	}

	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		ShowPopup();
	}

	void ShowPopup()
	{
		PizzasCollectionView.ScrollTo(0);
		var simplePopup = new PopupTutorial();
		this.ShowPopup(simplePopup);
	}
}

