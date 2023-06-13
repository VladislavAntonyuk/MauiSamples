namespace MauiShellCustomization;

using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		BindingContext = this;
	}

	public ICommand CenterViewCommand { get; } = new Command(async () => await Toast.Make("CenterViewCommand invoked!").Show());

}