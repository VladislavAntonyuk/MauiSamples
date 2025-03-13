namespace MauiMultiWindow;

using CommunityToolkit.Maui.Alerts;

public partial class MainPage : ContentPage
{
	private int counter;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OpenClicked(object sender, EventArgs e)
	{
		var page = new SecondPage
		{
			Title = "Second Page",
			Content = new Button
			{
				Text = "Show Active Windows",
				Command = new Command(async () =>
				{
					await Application.Current.GetActiveWindow().Page.DisplayAlert(
						"Active Windows",
						string.Join(Environment.NewLine, Application.Current.Windows.Select(x => $"{x.Title} - {x.IsActive()}")),
						"OK");
				})
			}
		};
		var newWindow = new WindowEx(page)
		{
			Title = $"New Window {counter++}"
		};
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
		var windows = Application.Current?.Windows.Except([Application.Current.GetActiveWindow()]).ToArray();
		foreach (var window in windows ?? [])
		{
			Application.Current?.CloseWindow(window);
		}
	}
}