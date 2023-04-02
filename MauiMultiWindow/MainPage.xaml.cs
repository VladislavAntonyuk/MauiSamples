namespace MauiMultiWindow;

using CommunityToolkit.Maui.Alerts;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OpenClicked(object sender, EventArgs e)
	{
		var newWindow = new Window(new SecondPage());
		Application.Current?.OpenWindow(newWindow);
	}

	private async void OpenModalClicked(object sender, EventArgs e)
	{
		var view = new ModalWindow<string>
		{
			Content = new VerticalStackLayout
			{
				new Label
				{
					Text = "I am a modal window"
				}
			},
			SubmitContentAction = async () =>
			{
				await Task.Delay(1000);
				return "Done";
			},
			SubmitContent = "Submit",
			CancelContent = "Close"
		};
		var result = await GetParentWindow().OpenModalWindowAsync(view);
		await Task.Delay(100);
		await Toast.Make($"Modal window is closed with result: {result}").Show();
	}

	private void CloseAllClicked(object sender, EventArgs e)
	{
		var windows = Application.Current?.Windows.Skip(1).ToArray();
		foreach (var window in windows ?? Array.Empty<Window>())
		{
			Application.Current?.CloseWindow(window);
		}
	}
}