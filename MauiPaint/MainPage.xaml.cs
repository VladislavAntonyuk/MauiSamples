namespace MauiPaint;

using System.Windows.Input;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageViewModel, IDeviceInfo deviceInfo)
	{
		InitializeComponent();
		if (deviceInfo.Platform == DevicePlatform.Android || deviceInfo.Platform == DevicePlatform.iOS)
		{
			AddToolbarItem("New", mainPageViewModel.NewCommand);
			AddToolbarItem("Open", mainPageViewModel.OpenCommand);
			AddToolbarItem("Save Project", mainPageViewModel.SaveCommand);
			AddToolbarItem("Save Image", mainPageViewModel.SaveImageCommand);
			AddToolbarItem("Toggle Theme", mainPageViewModel.ToggleThemeCommand);
			AddToolbarItem("Help", mainPageViewModel.HelpCommand);
			AddToolbarItem("About", mainPageViewModel.AboutCommand);
		}

		BindingContext = viewModel = mainPageViewModel;
	}
	
	public void RotateSlideValueChanged(object sender, ValueChangedEventArgs args)
	{
		viewModel.RotateCommand.Execute(args.NewValue);
	}

	// https://github.com/dotnet/maui/issues/6303
	readonly MainPageViewModel viewModel;

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
	public void PasteFromClipboardCommand(object sender, EventArgs args)
	{
		viewModel.PasteFromClipboardCommand.Execute(this);
	}
	public void HelpCommand(object sender, EventArgs args)
	{
		viewModel.HelpCommand.Execute(this);
	}
	public void AboutCommand(object sender, EventArgs args)
	{
		viewModel.AboutCommand.Execute(this);
	}

	void AddToolbarItem( string text, ICommand command)
	{
		var toolbarItem = new ToolbarItem()
		{
			Text = text,
			Command = command
		};
		ToolbarItems.Add(toolbarItem);
	}
}