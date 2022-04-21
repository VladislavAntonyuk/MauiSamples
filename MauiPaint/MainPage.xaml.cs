namespace MauiPaint;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();
		BindingContext = viewModel = mainPageViewModel;
    }

	// https://github.com/dotnet/maui/issues/6303
	MainPageViewModel viewModel;
    public void NewCommand(object sender, EventArgs args)
    {
		viewModel.NewCommand.Execute(this);
    }
    public void OpenCommand(object sender, EventArgs args)
    {
		viewModel.OpenCommand.Execute(this);
    }
    public void SaveCommand(object sender, EventArgs args)
    {
		viewModel.SaveCommand.Execute(this);
    }
    public void SaveImageCommand(object sender, EventArgs args)
    {
		viewModel.SaveImageCommand.Execute(this);
    }
    public void QuitCommand(object sender, EventArgs args)
    {
		viewModel.QuitCommand.Execute(this);
    }
    public void ToggleThemeCommand(object sender, EventArgs args)
    {
		viewModel.ToggleThemeCommand.Execute(this);
    }
    public void HelpCommand(object sender, EventArgs args)
    {
		viewModel.HelpCommand.Execute(this);
    }
    public void AboutCommand(object sender, EventArgs args)
    {
		viewModel.AboutCommand.Execute(this);
    }
}