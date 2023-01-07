namespace MauiMarkdown;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await using var stream = await FileSystem.OpenAppPackageFileAsync("dotnet_maui.md");
		using var reader = new StreamReader(stream);
		Editor.Text = await reader.ReadToEndAsync();
	}
}