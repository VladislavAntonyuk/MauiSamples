#pragma warning disable CS0169
namespace MauiMarkdown;

using Maui.BindableProperty.Generator.Core;

public partial class MarkdownGraphicsView : GraphicsView, IDisposable
{
	private PointF lastInteractionPoint;

	[AutoBindable(OnChanged = nameof(OnTextChanged))]
	private string? text;

	[AutoBindable(OnChanged = nameof(OnFontSizeChanged))]
	private float fontSize;

	[AutoBindable(OnChanged = nameof(OnFontColorChanged))]
	private Color fontColor = Colors.Black;

	private readonly MarkdownDrawable drawable;

	public MarkdownGraphicsView()
	{
		StartInteraction += OnStartInteraction;
		DragInteraction += OnDragInteraction;

		Drawable = drawable = new MarkdownDrawable();
	}

	private void OnTextChanged(string? oldValue, string? newValue)
	{
		Render();
	}
	private void OnFontSizeChanged(float oldValue, float newValue)
	{
		Render();
	}
	private void OnFontColorChanged(Color oldValue, Color newValue)
	{
		Render();
	}

	private void Render()
	{
		drawable.Text = Text;
		drawable.Size = new SizeF((float)Width, (float)Height);
		drawable.FontSize = FontSize;
		drawable.FontColor = FontColor;
		Invalidate();
	}

	private void OnStartInteraction(object? sender, TouchEventArgs e)
	{
		lastInteractionPoint = e.Touches[0];
	}

	private void OnDragInteraction(object? sender, TouchEventArgs e)
	{
		drawable.Offset = new SizeF(drawable.Offset.Width, drawable.Offset.Height - (lastInteractionPoint.Y - e.Touches[0].Y) / 50);
		Render();
	}

	public void Dispose()
	{
		StartInteraction -= OnStartInteraction;
		DragInteraction -= OnDragInteraction;
	}
}