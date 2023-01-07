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
		bool writeEndOfLines = true;

		if (codeBlock.Lines.Lines != null)
		{
			var lines = codeBlock.Lines;
			var slices = lines.Lines;
			for (int i = 0; i < lines.Count; i++)
			{
				if (!writeEndOfLines && i > 0)
				{
					renderer.WriteLine();
				}

				renderer.Write(ref slices[i].Slice);
				if (writeEndOfLines)
				{
					renderer.WriteLine();
				}
			}
		}

		var length = renderer.Count - start;
		renderer.Call("AddTextRun", start, length, attributes);
	}
}