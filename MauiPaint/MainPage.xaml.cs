namespace MauiPaint;

using System.Windows.Input;
using Microsoft.Maui.Platform;

public partial class MainPage : ContentPage
{
#if MACCATALYST || WINDOWS
	private readonly MainPageViewModel mainPageViewModel;
#endif

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
		this.mainPageViewModel = mainPageViewModel;
		Loaded += MainPage_Loaded;
	}

	private void MainPage_Loaded(object? sender, EventArgs e)
	{
		if (Handler?.MauiContext != null)
		{
			var uiElement = this.ToPlatform(Handler.MauiContext);
			DragDropHelper.RegisterDragDrop(uiElement, async stream =>
			{
				await mainPageViewModel.OpenFile(stream, CancellationToken.None);
			});
		}
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