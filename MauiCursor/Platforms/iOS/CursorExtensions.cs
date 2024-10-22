namespace MauiCursor;

using CoreGraphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Platform;
using UIKit;

public static class CursorExtensions
{
	public static void SetCustomCursor(this VisualElement visualElement, CursorIcon cursor, IMauiContext? mauiContext)
	{
		ArgumentNullException.ThrowIfNull(mauiContext);
		var view = visualElement.ToPlatform(mauiContext);
		view.UserInteractionEnabled = true;
		foreach (var interaction in view.Interactions.OfType<UIPointerInteraction>())
		{
			view.RemoveInteraction(interaction);
		}

		view.AddInteraction(new UIPointerInteraction(new PointerInteractionDelegate()));
	}

	class PointerInteractionDelegate : UIPointerInteractionDelegate
	{
		public override UIPointerStyle GetStyleForRegion(UIPointerInteraction interaction, UIPointerRegion region)
		{
			string pathData = "M14.9263942,24.822524 C15.7714904,24.822524 16.3700962,24.0948077 16.804375,22.9680048 L24.4925481,2.88509615 C24.7038462,2.34516827 24.8211538,1.86391827 24.8211538,1.46485577 C24.8211538,0.701899038 24.3516827,0.232403846 23.5887019,0.232403846 C23.1896635,0.232403846 22.7084135,0.349783654 22.1685096,0.561057692 L1.97987981,8.29608173 C0.993942308,8.67168269 0.230995192,9.2703125 0.230995192,10.1271394 C0.230995192,11.2069952 1.05262019,11.5708654 2.17942308,11.91125 L8.51769231,13.8362019 C9.26889423,14.0709615 9.67971154,14.047476 10.1961538,13.5779808 L23.0605769,1.54699519 C23.2129808,1.40615385 23.3891827,1.42963942 23.5182692,1.53526442 C23.6355769,1.65264423 23.6473558,1.82870192 23.5064904,1.98129808 L11.5107692,14.9043269 C11.0647356,15.3855529 11.0295192,15.7846394 11.252524,16.5710337 L13.1187981,22.7684615 C13.4709375,23.9539423 13.8347837,24.822524 14.9263942,24.822524 Z";
			var pathGeometry = new PathGeometryConverter().ConvertFromString(pathData) as PathGeometry;
			var path = UIBezierPath.FromPath(pathGeometry?.ToCGPath() ?? new CGPath());
			return UIPointerStyle.Create(UIPointerShape.Create(path), UIAxis.Both);
		}
	}
}

public static class GeometryExtensions
{
	public static CGPath ToCGPath(this PathGeometry pathGeometry)
	{
		var path = new CGPath();

		CGAffineTransform transform = CGAffineTransform.MakeIdentity();

		foreach (PathFigure pathFigure in pathGeometry.Figures)
		{
			path.MoveToPoint(transform, pathFigure.StartPoint);
			Point lastPoint = pathFigure.StartPoint;

			foreach (PathSegment pathSegment in pathFigure.Segments)
			{
				// LineSegment
				if (pathSegment is LineSegment lineSegment)
				{
					path.AddLineToPoint(transform, lineSegment.Point);
					lastPoint = lineSegment.Point;
				}
				// PolyLineSegment
				else if (pathSegment is PolyLineSegment polylineSegment)
				{
					PointCollection points = polylineSegment.Points;

					foreach (var point in points)
					{
						path.AddLineToPoint(transform, point);
					}

					lastPoint = points.Count > 0 ? points[^1] : Point.Zero;
				}

				// BezierSegment
				if (pathSegment is BezierSegment segment)
				{
					path.AddCurveToPoint(
						transform,
						segment.Point1,
						segment.Point2,
						segment.Point3);

					lastPoint = segment.Point3;
				}
				// PolyBezierSegment
				else if (pathSegment is PolyBezierSegment polyBezierSegment)
				{
					PointCollection points = polyBezierSegment.Points;

					if (points.Count >= 3)
					{
						for (int i = 0; i < points.Count; i += 3)
						{
							path.AddCurveToPoint(
								transform,
								points[i],
								points[i + 1],
								points[i + 2]);
						}
					}

					lastPoint = points.Count > 0 ? points[^1] : Point.Zero;
				}

				// QuadraticBezierSegment
				if (pathSegment is QuadraticBezierSegment bezierSegment)
				{
					path.AddQuadCurveToPoint(
						transform,
						new nfloat(bezierSegment.Point1.X),
						new nfloat(bezierSegment.Point1.Y),
						new nfloat(bezierSegment.Point2.X),
						new nfloat(bezierSegment.Point2.Y));

					lastPoint = bezierSegment.Point2;
				}
				// PolyQuadraticBezierSegment
				else if (pathSegment is PolyQuadraticBezierSegment polyBezierSegment)
				{
					PointCollection points = polyBezierSegment.Points;

					if (points.Count >= 2)
					{
						for (int i = 0; i < points.Count; i += 2)
						{
							path.AddQuadCurveToPoint(
								transform,
								new nfloat(points[i + 0].X),
								new nfloat(points[i + 0].Y),
								new nfloat(points[i + 1].X),
								new nfloat(points[i + 1].Y));
						}
					}

					lastPoint = points.Count > 0 ? points[^1] : Point.Zero;
				}
				// ArcSegment
				else if (pathSegment is ArcSegment arcSegment)
				{
					List<Point> points = new List<Point>();

					GeometryHelper.FlattenArc(
						points,
						lastPoint,
						arcSegment.Point,
						arcSegment.Size.Width,
						arcSegment.Size.Height,
						arcSegment.RotationAngle,
						arcSegment.IsLargeArc,
						arcSegment.SweepDirection == SweepDirection.CounterClockwise,
						1);

					CGPoint[] cgpoints = new CGPoint[points.Count];

					for (int i = 0; i < points.Count; i++)
						cgpoints[i] = transform.TransformPoint(points[i]);

					path.AddLines(cgpoints);

					lastPoint = points.Count > 0 ? points[^1] : Point.Zero;
				}
			}

			if (pathFigure.IsClosed)
			{
				path.CloseSubpath();
			}
		}

		return path;
	}
}