namespace MauiConditionView;

public partial class MainPage : ContentPage
{
	public int Count
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged();
		}
	}
	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
	}
}