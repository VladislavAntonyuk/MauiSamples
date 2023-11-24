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
			Content = new Label() { Text = "Tab1 Label" },
			Icon = "cat.png"
		});
		Tabs.Add(new Tab()
		{
			Title = "Tab2",
			Content = new Label() { Text = "Tab2 Label" },
			Icon = "dog.png"
		});
		Tabs2.Add(new Tab()
		{
			Title = "Tab1",
			Content = new Label() { Text = "Tab1 Label" },
			Icon = "cat.png"
		});
		Tabs2.Add(new Tab()
		{
			Title = "Tab2",
			Content = new Label() { Text = "Tab2 Label" },
			Icon = "dog.png"
		});
		Tabs3.Add(new Tab()
		{
			Title = "Tab1",
			Content = new Label() { Text = "Tab1 Label" },
			Icon = "cat.png"
		});
		Tabs3.Add(new Tab()
		{
			Title = "Tab2",
			Content = new Label() { Text = "Tab2 Label" },
			Icon = "dog.png"
		});
		SelectedTab = Tabs2[0];
	}
}