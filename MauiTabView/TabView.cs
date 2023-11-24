#pragma warning disable CS0169 // Field is never used
namespace MauiTabView;

using System.Collections.ObjectModel;
using Maui.BindableProperty.Generator.Core;

public partial class Tab : View
{
	[AutoBindable]
	private ImageSource? icon;

	[AutoBindable]
	private string title = string.Empty;

	[AutoBindable(DefaultValue = "new ContentView()")]
	private IView content = new ContentView();
}

public partial class TabView : VerticalStackLayout
{
	[AutoBindable(DefaultValue = "new System.Collections.ObjectModel.ObservableCollection<Tab>()", OnChanged = "OnTabsChanged")]
	private ObservableCollection<Tab> tabs = new();

	[AutoBindable(DefaultValue = "-1", OnChanged = "OnActiveTabIndexChanged")]
	private int activeTabIndex;

	void OnTabsChanged()
	{
		Children.Clear();
		Children.Add(BuildTabs());
		OnActiveTabIndexChanged();
		ActiveTabIndex = Tabs.Count > 0 ? 0 : -1;
	}

	public TabView()
	{
		Loaded += TabView_Loaded;
	}

	private void TabView_Loaded(object? sender, EventArgs e)
	{
		OnTabsChanged();
	}

	void OnActiveTabIndexChanged()
	{
		var activeTab = GetActiveTab();
		if (activeTab is null)
		{
			return;
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
		if (Tabs.Count < ActiveTabIndex || ActiveTabIndex < 0)
		{
			return null;
		}

		var activeTab = Tabs[ActiveTabIndex];
		return activeTab.Content;
	}
}