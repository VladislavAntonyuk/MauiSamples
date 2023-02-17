namespace MauiMarkdown;

using Markdig.Syntax;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui.Graphics.Text.Renderer;

public class MauiCodeBlockRenderer : AttributedTextObjectRenderer<CodeBlock>
{
	protected override void Write(
		AttributedTextRenderer renderer,
		CodeBlock codeBlock)
	{
		var start = renderer.Count;
		var attributes = new TextAttributes();
		attributes.SetBackgroundColor("#f5f2f0");
		var randomColor = new Color(Random.Shared.Next(100), Random.Shared.Next(100), Random.Shared.Next(100));
		attributes.SetForegroundColor(randomColor.ToArgbHex());

		if (codeBlock.Lines.Lines != null)
		{
			var lines = codeBlock.Lines;
			var slices = lines.Lines;
			for (int i = 0; i < lines.Count; i++)
			{
				renderer.Write(ref slices[i].Slice);
				renderer.WriteLine();
			}
		}

		var length = renderer.Count - start;
		renderer.Call("AddTextRun", start, length, attributes);
	}
}