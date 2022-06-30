#pragma warning disable CS0169
namespace CardLayout;

using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Controls.Shapes;

public partial class CardView : ContentView
{
	private readonly VerticalStackLayout container = new();

	[AutoBindable(OnChanged = nameof(UpdateView))]
	private IView? cardContent;
	[AutoBindable(OnChanged = nameof(UpdateView))]
	private IView? footer;
	[AutoBindable(OnChanged = nameof(UpdateView))]
	private IView? header;

	private void UpdateView(IView? oldValue, IView? newValue)
	{
		if (oldValue is null && newValue is not null)
		{
			container.Add(newValue);
			var border = new Border()
			{
				Content = container,
				StrokeShape = new RoundRectangle() { CornerRadius = new CornerRadius(10) },
				StrokeThickness = 4,
				Stroke = Brush.CadetBlue
			};
			Content = border;
		}
	}

	public CardView()
	{
		container.Background = Brush.Gray;
		container.Padding = 10;
	}
}