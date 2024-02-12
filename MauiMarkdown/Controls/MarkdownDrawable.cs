namespace MauiMarkdown;

using Markdig;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui.Graphics.Text.Renderer;
using Font = Microsoft.Maui.Graphics.Font;

public class MarkdownDrawable : IDrawable
{
	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		canvas.Font = Font.Default;
		canvas.FontSize = FontSize;
		canvas.FontColor = FontColor;
		var attributedText = Read(Text);
#if WINDOWS
		canvas.DrawString(attributedText.Text, 0, Offset.Height, Math.Max(0, Size.Width), Math.Max(0, Size.Height), HorizontalAlignment.Left, VerticalAlignment.Top);
#else
		canvas.DrawText(attributedText, 0, Offset.Height, Size.Width, Size.Height);
#endif
	}

	private static IAttributedText Read(string? text)
	{
		var renderer = new AttributedTextRenderer();
		renderer.ObjectRenderers.Add(new MauiCodeInlineRenderer());
		renderer.ObjectRenderers.Add(new MauiCodeBlockRenderer());
		renderer.ObjectRenderers.Add(new MauiHeadingRenderer());
		var builder = new MarkdownPipelineBuilder()
					  .UseEmojiAndSmiley()
					  .UseEmphasisExtras();
		var pipeline = builder.Build();
		Markdown.Convert(text ?? string.Empty, renderer, pipeline);
		return renderer.GetAttributedText();
	}

	public SizeF Offset { get; set; }
	public string? Text { get; set; }
	public SizeF Size { get; set; }
	public float FontSize { get; set; }
	public Color FontColor { get; set; } = Colors.Black;
}