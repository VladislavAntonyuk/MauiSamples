namespace TutorialHelp;

using CommunityToolkit.Maui.Views;

public partial class PopupTutorial : Popup
{
	public PopupTutorial(Size size)
	{
		InitializeComponent();
#if WINDOWS
		Size = size;
#else
		var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
		Size = new Size(mainDisplayInfo.Width / mainDisplayInfo.Density, mainDisplayInfo.Height / mainDisplayInfo.Density);
#endif
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		Close();
	}
}