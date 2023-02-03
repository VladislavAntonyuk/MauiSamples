namespace MauiMarkdown;

using Markdig;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui.Graphics.Text.Renderer;
using Font = Microsoft.Maui.Graphics.Font;

public class MarkdownDrawable : IDrawable
{
	private readonly string text;
	private readonly Color fontColor;
	private readonly float fontSize;
	private readonly int markdownWidth;
	private readonly int markdownHeight;

	public MarkdownDrawable(string text, Color fontColor, double fontSize, double markdownWidth, double markdownHeight)
	{
		this.text = text;
		this.fontColor = fontColor;
		this.fontSize = (float)fontSize;
		this.markdownWidth = (int)markdownWidth;
		this.markdownHeight = (int)markdownHeight;
	}

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		canvas.Font = Font.Default;
		canvas.FontSize = fontSize;
		canvas.FontColor = fontColor;
		var attributedText = Read(text);
		canvas.DrawText(attributedText, 0, 0, markdownWidth, markdownHeight);
	}

	private static IAttributedText Read(string text)
	{
		var renderer = new AttributedTextRenderer();
		renderer.ObjectRenderers.Add(new MauiCodeInlineRenderer());
		renderer.ObjectRenderers.Add(new MauiCodeBlockRenderer());
		renderer.ObjectRenderers.Add(new MauiHeadingRenderer());
		var builder = new MarkdownPipelineBuilder()
					  .UseEmojiAndSmiley()
					  .UseEmphasisExtras();
		var pipeline = builder.Build();
		Markdig.Markdown.Convert(text, renderer, pipeline);
		return renderer.GetAttributedText();
	}
}