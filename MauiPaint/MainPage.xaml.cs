namespace MauiPaint;

using System.Windows.Input;

#if WINDOWS
using CommunityToolkit.Mvvm.Messaging;
public partial class MainPage : ContentPage, IRecipient<DropItemMessage>
#else
public partial class MainPage : ContentPage
#endif
{
#if WINDOWS
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

#if WINDOWS
		this.mainPageViewModel = mainPageViewModel;
		Loaded += (sender, args) =>
		{
			DrawingView.RegisterDrop(Handler?.MauiContext);
			WeakReferenceMessenger.Default.Register<DropItemMessage>(this);
		};

		Unloaded += (sender, args) =>
		{
			DrawingView.UnRegisterDrop(Handler?.MauiContext);
			WeakReferenceMessenger.Default.Unregister<DropItemMessage>(this);
		};
#endif

#if MACCATALYST
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

#if WINDOWS
	public async void Receive(DropItemMessage message)
	{
		await mainPageViewModel.OpenFile(message.Value, CancellationToken.None);
	}
#endif
}