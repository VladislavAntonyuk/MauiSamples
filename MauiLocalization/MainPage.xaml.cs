namespace MauiLocalization;

using System.Globalization;
using Resources.Localization;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		LanguagePicker.Items.Add("en-us");
		LanguagePicker.Items.Add("uk-ua");
		LanguagePicker.SelectedIndex = 0;
		BindingContext = this;
	}

	private void LanguageChanged(object sender, EventArgs e)
	{
		LocalizationResourceManager.Instance.SetCulture(new CultureInfo(LanguagePicker.Items[LanguagePicker.SelectedIndex]));
	}
}