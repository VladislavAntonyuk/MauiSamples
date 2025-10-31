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
	public partial Tab SelectedTab { get; set; }

	public ObservableCollection<Tab> Tabs { get; set; } = new();
	public ObservableCollection<Tab> Tabs2 { get; set; } = new();
	public ObservableCollection<Tab> Tabs3 { get; set; } = new();
	public ObservableCollection<Tab> Tabs4 { get; set; } = new();

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
		
		// Tabs4 shares the same Tab objects as Tabs3 to demonstrate the fix
		// This would previously cause "The specified child already has a parent" exception
		foreach (var tab in Tabs3)
		{
			Tabs4.Add(tab);
		}
		
		SelectedTab = Tabs2[0];
	}
}