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

	private void OnCounterClicked(object sender, EventArgs e)
	{
		Count++;

		CounterBtn.Text = Count == 1 ? $"Clicked {Count} time" : $"Clicked {Count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}