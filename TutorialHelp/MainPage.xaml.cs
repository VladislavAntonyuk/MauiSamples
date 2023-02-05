namespace TutorialHelp;

using CommunityToolkit.Maui.Views;

public partial class MainPage : ContentPage
{
	int count;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		CounterLabel.Text = $"Current count: {count}";

		SemanticScreenReader.Announce(CounterLabel.Text);
		ShowPopup();
	}

	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		ShowPopup();
	}

	void ShowPopup()
	{
		var simplePopup = new PopupTutorial(new Size(Width, Height));
		this.ShowPopup(simplePopup);
	}
}