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

		BindingContext = mainPageViewModel;
#if MACCATALYST || WINDOWS
		Loaded += (sender, args) =>
		{
			DrawingView.RegisterDrop(Handler?.MauiContext, async stream =>
			{
				await mainPageViewModel.OpenFile(stream, CancellationToken.None);
			});
		};

		Unloaded += (sender, args) =>
		{
			DrawingView.UnRegisterDrop(Handler?.MauiContext);
		};
#endif
	}

	void AddToolbarItem(string text, ICommand command)
	{
		var toolbarItem = new ToolbarItem()
		{
			Text = text,
			Command = command
		};
		ToolbarItems.Add(toolbarItem);
	}
}