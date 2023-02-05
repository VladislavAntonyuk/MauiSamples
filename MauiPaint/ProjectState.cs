namespace MauiPaint;

using CommunityToolkit.Maui.Core;
using Figures;

public class ProjectState
{
	public IFigure[] Figures { get; set; } = Array.Empty<IFigure>();
	public IDrawingLine[] Lines { get; set; } = Array.Empty<IDrawingLine>();
	public Color LineColor { get; set; } = Colors.Black;
	public float LineWidth { get; set; } = 5;
	public Brush Background { get; set; } = Brush.White;
}