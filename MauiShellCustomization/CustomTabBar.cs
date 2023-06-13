#pragma warning disable CS0169
#pragma warning disable CS0414
namespace MauiShellCustomization;

using System.Windows.Input;
using Maui.BindableProperty.Generator.Core;

public partial class CustomTabBar : TabBar
{
	[AutoBindable]
	private ICommand? centerViewCommand;

	[AutoBindable]
	private ImageSource? centerViewImageSource;

	[AutoBindable]
	private string? centerViewText;

	[AutoBindable]
	private bool centerViewVisible;

	[AutoBindable]
	public Color? centerViewBackgroundColor;
}