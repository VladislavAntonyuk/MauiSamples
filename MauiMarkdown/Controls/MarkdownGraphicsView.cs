#pragma warning disable CS0169
namespace MauiMarkdown;

using Maui.BindableProperty.Generator.Core;

public partial class MarkdownGraphicsView : GraphicsView
{
	[AutoBindable(OnChanged = nameof(OnTextChanged))]
	private string? text;

	[AutoBindable(OnChanged = nameof(OnFontSizeChanged))]
	private float fontSize;

	[AutoBindable(OnChanged = nameof(OnFontColorChanged))]
	private Color fontColor = Colors.Black;

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
		Drawable = new MarkdownDrawable(Text ?? string.Empty, FontColor, FontSize, Width, Height);
	}
}