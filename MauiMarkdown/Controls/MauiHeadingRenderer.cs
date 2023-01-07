namespace MauiMarkdown;

using Markdig.Syntax;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui.Graphics.Text.Renderer;

public class MauiHeadingRenderer : AttributedTextObjectRenderer<HeadingBlock>
{
	protected override void Write(
		AttributedTextRenderer renderer,
		HeadingBlock block)
	{
		var start = renderer.Count;
		var attributes = new TextAttributes();
		attributes.SetFontSize(block.Level switch
		{
			1 => 24,
			2 => 20,
			3 => 16,
			4 => 14,
			5 => 12,
			6 => 10,
			_ => 8
		});
		if (block.Line > 0)
		{
			renderer.WriteLine();
		}

		renderer.WriteLeafInline(block);
		renderer.WriteLine();
		var length = renderer.Count - start;
		renderer.Call("AddTextRun", start, length, attributes);
	}
}