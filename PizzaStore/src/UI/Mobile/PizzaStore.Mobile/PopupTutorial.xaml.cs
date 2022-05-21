namespace PizzaStore.Mobile;

using CommunityToolkit.Maui.Views;

public partial class PopupTutorial : Popup
{
	public PopupTutorial()
	{
		InitializeComponent();
		Size = new Size(
			0.9 * DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density,
			0.9 * DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		Close();
	}
}