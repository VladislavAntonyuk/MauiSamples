#pragma warning disable CS0169 // Field is never used
namespace MauiTabView;

using System.Collections.ObjectModel;
using Maui.BindableProperty.Generator.Core;
using ILayout = Microsoft.Maui.ILayout;

public partial class Tab : ContentView
{
	[AutoBindable]
	private ImageSource? icon;

	[AutoBindable]
	private string title = string.Empty;
}

public partial class TabView : VerticalStackLayout
{
	public static readonly BindableProperty ActiveTabIndexProperty = BindableProperty.Create(nameof(ActiveTabIndex), typeof(int), typeof(TabView), -1, propertyChanged: OnActiveTabIndexChanged);

	private static void OnActiveTabIndexChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var tabView = (TabView)bindable;
		tabView.OnActiveTabIndexChanged();
	}

	public static readonly BindableProperty TabsProperty = BindableProperty.Create(nameof(Tabs), typeof(ObservableCollection<Tab>), typeof(TabView));

	void OnTabsChanged()
	{
		Children.Clear();
		Children.Add(BuildTabs());
		ActiveTabIndex = -1;
	}

	public TabView()
	{
		Tabs = new ObservableCollection<Tab>();
		Loaded += OnLoaded;
	}

	private void OnLoaded(object? sender, EventArgs e)
	{
		OnTabsChanged();
		Loaded -= OnLoaded;
	}

	void OnActiveTabIndexChanged()
	{
		var activeTab = GetActiveTab();
		if (activeTab is null)
		{
			return;
		}

		// Remove the view from its current parent if it has one
		// This is necessary when multiple TabView instances share the same Tab objects
		if (activeTab.Parent is ILayout parentLayout)
		{
			parentLayout.Remove(activeTab);
		}

		if (Children.Count == 1)
		{
			Children.Add(activeTab);
		}
		else
		{
			Children[1] = activeTab;
		}
	}

	IView BuildTabs()
	{
		var view = new HorizontalStackLayout()
		{
			HorizontalOptions = LayoutOptions.Center,
			Spacing = 10
		};
		for (var index = 0; index < Tabs.Count; index++)
		{
			var tab = Tabs[index];
			var index1 = index;
			var tabHeader = new VerticalStackLayout()
			{
				GestureRecognizers =
				{
					new TapGestureRecognizer()
					{
						Command = new Command((() => ActiveTabIndex = index1))
					}
				}
			};
			tabHeader.Children.Add(new Image() { Source = tab.Icon, HorizontalOptions = LayoutOptions.Center, WidthRequest = 30, HeightRequest = 30 });
			tabHeader.Children.Add(new Label() { Text = tab.Title, HorizontalOptions = LayoutOptions.Center });
			view.Children.Add(tabHeader);
		}

		return view;
	}

	IView? GetActiveTab()
	{
		if (Tabs.Count <= ActiveTabIndex || ActiveTabIndex < 0)
		{
			return null;
		}

		var activeTab = Tabs[ActiveTabIndex];
		return activeTab.Content;
	}

	public ObservableCollection<Tab> Tabs
	{
		get => (ObservableCollection<Tab>)GetValue(TabsProperty);
		set => SetValue(TabsProperty, value);
	}

	public int ActiveTabIndex
	{
		get => (int)GetValue(ActiveTabIndexProperty);
		set => SetValue(ActiveTabIndexProperty, value);
	}
}