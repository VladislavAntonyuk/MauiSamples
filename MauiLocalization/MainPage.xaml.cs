namespace MauiLocalization;

using System.Globalization;
using Resources.Localization;

public partial class MainPage : ContentPage
{
	public string NameStringKey { get; set; } = "Intro";

	public MainPage()
	{
		InitializeComponent();
		LanguagePicker.Items.Add("en-us");
		LanguagePicker.Items.Add("uk-ua");
		LanguagePicker.SelectedIndex = 0;
		LocalizationResourceManager = LocalizationResourceManager.Instance;
		CodeBehindTranslator.SetBinding(Label.TextProperty, new Binding("LocalizationResourceManager[Intro]", BindingMode.OneWay));
		BindingContext = this;
	}

	private void LanguageChanged(object sender, EventArgs e)
	{
		LocalizationResourceManager.Instance.SetCulture(new CultureInfo(LanguagePicker.Items[LanguagePicker.SelectedIndex]));
		CodeBehindTranslator2.Text = LocalizationResourceManager.Instance["Intro"].ToString();
	}

	public LocalizationResourceManager LocalizationResourceManager { get; }
}