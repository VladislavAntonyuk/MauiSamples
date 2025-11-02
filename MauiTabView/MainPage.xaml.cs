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

	public ObservableCollection<Tab> Tabs1 { get; set; } = new();
	public ObservableCollection<Tab> Tabs2 { get; set; } = new();
	public ObservableCollection<Tab> Tabs3 { get; set; } = new();

	public MainViewModel()
	{
		Tabs1.Add(new Tab()
		{
			Title = "Tab11",
			Content = new Label() { Text = "Tab11 Label" },
			Icon = "cat.png"
		});
		Tabs1.Add(new Tab()
		{
			Title = "Tab12",
			Content = new Label() { Text = "Tab12 Label" },
			Icon = "dog.png"
		});
		Tabs2.Add(new Tab()
		{
			Title = "Tab21",
			Content = new Label() { Text = "Tab21 Label" },
			Icon = "cat.png"
		});
		Tabs2.Add(new Tab()
		{
			Title = "Tab22",
			Content = new Label() { Text = "Tab22 Label" },
			Icon = "dog.png"
		});
		Tabs3.Add(new Tab()
		{
			Title = "Tab31",
			Content = new Label() { Text = "Tab31 Label" },
			Icon = "cat.png"
		});
		Tabs3.Add(new Tab()
		{
			Title = "Tab32",
			Content = new Label() { Text = "Tab32 Label" },
			Icon = "dog.png"
		});

		SelectedTab = Tabs2[0];
	}
}