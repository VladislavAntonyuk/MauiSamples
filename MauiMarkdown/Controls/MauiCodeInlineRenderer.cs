namespace MauiMarkdown;

using Markdig.Syntax.Inlines;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui.Graphics.Text.Renderer;

public class MauiCodeInlineRenderer : AttributedTextObjectRenderer<CodeInline>
{
	protected override void Write(
		AttributedTextRenderer renderer,
		CodeInline inlineBlock)
	{
		var start = renderer.Count;
		var attributes = new TextAttributes();
		attributes.SetForegroundColor("#d63384");
		attributes.SetFontSize(35f);
		renderer.Write(inlineBlock.Content);
		var length = renderer.Count - start;
		renderer.Call("AddTextRun", start, length, attributes);
	}
}