namespace AppContainer;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	public void LoadImage(byte[] imageData)
	{
		Img.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
	}

	private async void OnClicked(object sender, EventArgs e)
	{
		await Launcher.OpenAsync("https://vladislavantonyuk.github.io/");
	}
}