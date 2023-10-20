namespace MauiTabView;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		BindingContext = new MainViewModel();
	}
}

public partial class MainViewModel : ObservableObject
{
	[ObservableProperty]
	private Tab selectedTab;

	public ObservableCollection<Tab> Tabs { get; set; } = new();
	public ObservableCollection<Tab> Tabs2 { get; set; } = new();
	public ObservableCollection<Tab> Tabs3 { get; set; } = new();

	public MainViewModel()
	{
		Tabs.Add(new Tab()
		{
			Title = "Tab1",
			Content = new Label() { Text = "Tab1 Label" }
		});
		Tabs.Add(new Tab()
		{
			Title = "Tab2",
			Content = new Label() { Text = "Tab2 Label" }
		});
		Tabs2.Add(new Tab()
		{
			Title = "Tab1",
			Content = new Label() { Text = "Tab1 Label" }
		});
		Tabs2.Add(new Tab()
		{
			Title = "Tab2",
			Content = new Label() { Text = "Tab2 Label" }
		});
		Tabs3.Add(new Tab()
		{
			Title = "Tab1",
			Content = new Label() { Text = "Tab1 Label" }
		});
		Tabs3.Add(new Tab()
		{
			Title = "Tab2",
			Content = new Label() { Text = "Tab2 Label" }
		});
		SelectedTab = Tabs2[0];
	}
}