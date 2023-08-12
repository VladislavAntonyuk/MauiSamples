namespace MauiShellCustomization;

using System.Reflection;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;

public class CustomShellHandler : ShellHandler
{
	protected override ShellView CreatePlatformView()
	{
		var shellView = base.CreatePlatformView();
		return shellView;
	}

	protected override void ConnectHandler(ShellView platformView)
	{
		platformView.Loaded += ShellViewOnLoaded;
		platformView.LayoutUpdated += ShellView_LayoutUpdated;
		base.ConnectHandler(platformView);
	}

	protected override void DisconnectHandler(ShellView platformView)
	{
		platformView.Loaded -= ShellViewOnLoaded;
		platformView.LayoutUpdated -= ShellView_LayoutUpdated;
		base.DisconnectHandler(platformView);
	}

	private void ShellView_LayoutUpdated(object? sender, object e)
	{
		if (PlatformView.Content is MauiNavigationView content)
		{
			content.MenuItemTemplate = CreateNavigationViewItemDataTemplate();
			content.MenuItemContainerStyle = CreateNavItemViewContainerStyle();

			// Modify Page Container (Page1, Page2)
			var contentGrid = ContentGrid(content);
			if (contentGrid is not null)
			{
				contentGrid.CornerRadius = new CornerRadius(30);
				contentGrid.Margin = new Thickness(30);
			}
		}
	}

	private void ShellViewOnLoaded(object sender, RoutedEventArgs e)
	{
		if (!PlatformView.IsLoaded)
		{
			return;
		}

		if (PlatformView.Header is MauiToolbar header)
		{
			header.Margin = new Thickness(30);
		}

		if (PlatformView.Content is MauiNavigationView content)
		{
			// Modify Top Nav Area Items (Tab1, Tab2)
			var menuItemsGrid = MenuItemsGrid(content);
			if (menuItemsGrid is not null)
			{
				menuItemsGrid.Margin = new Thickness(30, 0, 30, 0);
				menuItemsGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
				menuItemsGrid.CornerRadius = new CornerRadius(30);
				menuItemsGrid.Background = Colors.LightBlue.ToPlatform();
			}
		}
	}

	private static DataTemplate CreateNavigationViewItemDataTemplate()
	{
		var xaml = """
		           <DataTemplate xmlns = 'http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
		             <NavigationViewItem BackgroundSizing = "OuterBorderEdge"
		           				Content = "{Binding Content}"
		           				Foreground = "{Binding Foreground}"
		           				Background = "{Binding Background}"
		           				IsSelected = "{Binding IsSelected, Mode=TwoWay}"
		           				MenuItemsSource = "{Binding MenuItemsSource}"
		           				Icon = "{Binding Icon}" />
		           </DataTemplate>
		           """;

		var dataTemplate = (DataTemplate)XamlReader.Load(xaml);
		return dataTemplate;
	}

	private static Style CreateNavItemViewContainerStyle()
	{
		var navViewItemContainerStyle = new Style(typeof(NavigationViewItem));
		navViewItemContainerStyle.Setters.Add(new Setter(Control.FontSizeProperty, 24));
		return navViewItemContainerStyle;
	}

	private static Grid? TopNavGrid(MauiNavigationView content)
	{
		var topNavAreaProperty = content.GetType()
		                                .GetProperty("TopNavMenuItemsHost",
		                                             BindingFlags.NonPublic | BindingFlags.Instance);
		return (((topNavAreaProperty?.GetValue(content) as ItemsRepeater)?.Parent as ScrollViewer)?.Parent as
			ItemsRepeaterScrollHost)?.Parent as Grid;
	}

	private static StackPanel? TopNavArea(MauiNavigationView content)
	{
		var topNavAreaProperty =
			content.GetType().GetProperty("TopNavArea", BindingFlags.NonPublic | BindingFlags.Instance);
		return topNavAreaProperty?.GetValue(content) as StackPanel;
	}

	private static Grid? MenuItemsGrid(MauiNavigationView content)
	{
		var topNavArea = TopNavArea(content);
		if (topNavArea is not null && topNavArea.Children.Count > 1)
		{
			return topNavArea.Children[1] as Grid;
		}

		return TopNavGrid(content);
	}

	private static Grid? ContentGrid(MauiNavigationView content)
	{
		var topNavAreaProperty =
			content.GetType().GetProperty("ContentGrid", BindingFlags.NonPublic | BindingFlags.Instance);
		return topNavAreaProperty?.GetValue(content) as Grid;
	}
}