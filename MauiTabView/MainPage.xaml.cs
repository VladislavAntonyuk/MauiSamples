namespace MauiTabView;

using System.Collections.ObjectModel;
using System.Globalization;

public partial class MainPage : ContentPage
{
	public ObservableCollection<Tab> Tabs { get; set; } = new();
	public ObservableCollection<Tab> Tabs2 { get; set; } = new();

	public MainPage()
	{
		InitializeComponent();
		Tabs.Add(new Tab()
		{
			Title = "Tab1",
			Content = new Label(){Text = "Tab1 Label"}
		});
		Tabs.Add(new Tab()
		{
			Title = "Tab2",
			Content = new Label(){Text = "Tab2 Label"}
		});
		Tabs2.Add(new Tab()
		{
			Title = "Tab1",
			Content = new Label(){Text = "Tab1 Label"}
		});
		Tabs2.Add(new Tab()
		{
			Title = "Tab2",
			Content = new Label(){Text = "Tab2 Label"}
		});
		BindingContext = this;
	}
}