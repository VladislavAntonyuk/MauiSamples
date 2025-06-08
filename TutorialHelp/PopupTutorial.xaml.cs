namespace TutorialHelp;

using CommunityToolkit.Maui.Views;

public partial class PopupTutorial : Popup
{
	public PopupTutorial(Size size)
	{
		InitializeComponent();
#if WINDOWS
		WidthRequest = size.Width;
		HeightRequest = size.Height;
#else
		var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
		WidthRequest = mainDisplayInfo.Width / mainDisplayInfo.Density;
		HeightRequest = mainDisplayInfo.Height / mainDisplayInfo.Density;
#endif
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await CloseAsync();
	}
}