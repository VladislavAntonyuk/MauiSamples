
using Microsoft.Maui.Controls;

namespace CirclePacking.HillClimbing;

using System.Numerics;

public class RandomPointGenerator
{
	private readonly Random _randy = new Random();

	public List<MyPoint> GetPointsInACircle2(int radius, int numberOfPoints)
	{
		var points = new List<MyPoint>();
		for (var pointIndex = 0; pointIndex < numberOfPoints; pointIndex++)
		{
			var distance = _randy.Next(radius);
			var angleInRadians = _randy.Next(360) / (2 * Math.PI);

			var x = (int)(distance * Math.Cos(angleInRadians));
			var y = (int)(distance * Math.Sin(angleInRadians));
			var randomPoint = new MyPoint(x, y);
			points.Add(randomPoint);
		}

		return points;
	}

	public List<Point> GetPointsInACircle(int radius, int numberOfPoints)
	{
		var points = new List<Point>();
		for (var pointIndex = 0; pointIndex < numberOfPoints; pointIndex++)
		{
			var distance = _randy.Next(radius);
			var angleInRadians = _randy.Next(360) / (2 * Math.PI);

			var x = (int)(distance * Math.Cos(angleInRadians));
			var y = (int)(distance * Math.Sin(angleInRadians));
			var randomPoint = new Point(x, y);
			points.Add(randomPoint);
		}

		return points;
	}

	public List<Point> MovePoints(List<Point> oldPoints, double eps)
	{
		var points = new List<Point>();
		foreach (var p in oldPoints)
		{
			var x = (_randy.Next(-100, 100) < 0 ? -1 : 1) * _randy.NextDouble() * eps;
			var y = (_randy.Next(-100, 100) < 0 ? -1 : 1) * _randy.NextDouble() * eps;
			points.Add(new Point(p.X + x, p.Y + y));
		}

		return points;
	}

	public List<Point> MovePoints2(List<Point> oldPoints, double eps)
	{
		var points = oldPoints;
		var i = _randy.Next(0, points.Count);
		var x = (_randy.Next(-100, 100) < 0 ? -1 : 1) * _randy.NextDouble() * eps;
		var y = (_randy.Next(-100, 100) < 0 ? -1 : 1) * _randy.NextDouble() * eps;
		points[i] = new Point(points[i].X + x, points[i].Y + y);

		return points;
	}

	public Pair Mutation(Pair pair, double eps)
	{
		/*foreach (var item in pair.Points)
		{
			var x = (_randy.Next(-100, 100) < 0 ? -1 : 1) * _randy.NextDouble() * eps;
			var y = (_randy.Next(-100, 100) < 0 ? -1 : 1) * _randy.NextDouble() * eps;
			item.X = item.X + x;
			item.Y = item.Y + y;
		}*/

		var i = new Random().Next(0, pair.Points.Count() - 1);
		var x = (_randy.Next(-100, 100) < 0 ? -1 : 1) * _randy.NextDouble() * eps;
		var y = (_randy.Next(-100, 100) < 0 ? -1 : 1) * _randy.NextDouble() * eps;
		pair.Points[i].X += (float)x;
		pair.Points[i].Y += (float)y;


		return pair;
	}
}

public class Circle
{
	public Color circleColor;
	public string? CircleName;
	public double Height = 100.0f;
	public Vector2 mCenter;
	public float mRadius;
	public double Width = 100.0f;

	public Circle(Vector2 nCenter, float nRadius)
	{
		mCenter = nCenter;
		mRadius = nRadius;
		circleColor = Color.FromRgb(156, 156, 156);
	}

	public double DistanceToCenter
	{
		get
		{
			var dx = mCenter.X - Width / 2;
			var dy = mCenter.Y - Height / 2;
			return Math.Sqrt(dx * dx + dy * dy);
		}
	}

	public bool Contains(double x, double y)
	{
		var dx = mCenter.X - x;
		var dy = mCenter.Y - y;
		return Math.Sqrt(dx * dx + dy * dy) <= mRadius;
	}

	public bool Intersects(Circle c)
	{
		var dx = c.mCenter.X - mCenter.X;
		var dy = c.mCenter.Y - mCenter.Y;
		var d = Math.Sqrt(dx * dx + dy * dy);
		return d < mRadius || d < c.mRadius;
	}
}

public interface ICirclePacker
{
	double Iterate();
	void Render();
}

public class HillClimbingCirclePacker : ICirclePacker
{
	public readonly float eps = 0.0001f;

	private readonly double radius;

	/// <summary>
	/// </summary>
	private bool min;

	/// <summary>
	///     Generates a number of Packing circles in the constructor.
	///     Random distribution is linear
	/// </summary>
	public HillClimbingCirclePacker(ICanvas hostCanvas, double radius, int pNumCircles)
	{
		HostCanvas = hostCanvas;
		this.radius = radius;
		AllCircles = new List<Circle>();
		// Create random circles
		AllCircles.Clear();
		var Rnd = new Random(DateTime.Now.Millisecond);
		for (var i = 0; i < pNumCircles; i++)
		{
			var nCenter = new Vector2((float)(radius + Rnd.NextDouble() * eps), (float)(radius + Rnd.NextDouble() * eps));
			AllCircles.Add(new Circle(nCenter, eps));
		}
	}

	public List<Circle> AllCircles { get; set; }
	public ICanvas HostCanvas { get; set; }

	public double Iterate()
	{
		var curRad = eps;
		while (!min)
		{
			foreach (var t in AllCircles)
			{
				t.mRadius += eps;
				curRad = t.mRadius;
			}

			// Sort circles based on the distance to center
			var sortedCircles = from c in AllCircles
								orderby c.DistanceToCenter descending
								select c;

			AllCircles = sortedCircles.ToList();

			var minSeparationSq = eps * eps;
			for (var i = 0; i < AllCircles.Count - 1; i++)
			{
				for (var j = i + 1; j < AllCircles.Count; j++)
				{
					if (i == j)
					{
						continue;
					}

					var AB = AllCircles[j].mCenter - AllCircles[i].mCenter;
					var r = AllCircles[i].mRadius + AllCircles[j].mRadius;

					// Length squared = (dx * dx) + (dy * dy);
					var d = AB.LengthSquared() - minSeparationSq;
					var minSepSq = Math.Min(d, minSeparationSq);
					d -= minSepSq;

					if (d < r * r - eps)
					{
						AB = Vector2.Normalize(AB);

						AB *= (float)(r - Math.Sqrt(d)) * 0.5f;

						AllCircles[j].mCenter += AB;
						AllCircles[i].mCenter -= AB;
					}
				}
			}

			var d1 = AllCircles.Select(t => new Point(t.mCenter.X, t.mCenter.Y)).Select(v1 => radius - Math.Sqrt(Math.Pow(v1.X - radius, 2) + Math.Pow(v1.Y - radius, 2))).ToList();

			d1.Sort();

			foreach (var t in AllCircles)
			{
				if (d1[0] < eps || d1[0] - t.mRadius < eps)
				{
					min = true;
				}
			}

			if (curRad > radius / 2)
			{
				min = true;
			}
		}

		return AllCircles[0].mRadius;
	}

	public void Render()
	{
		// HostCanvas.Children.Clear();
		// for (var i = 0; i < AllCircles.Count; i++)
		// {
		// 	var c = AllCircles[i];
		// 	// Just in case there are some NaN values out there
		// 	if (!c.mCenter.X.ToString().Contains("∞") && !c.mCenter.Y.ToString().Contains("∞"))
		// 	{
		// 		if (i < HostCanvas.Children.Count)
		// 		{
		// 			var e = (Ellipse)HostCanvas.Children[i];
		// 			e.Width = e.Height = c.mRadius * 2;
		// 			e.Fill = new SolidColorBrush(c.circleColor);
		// 			e.Stroke = Brushes.Black;
		// 			Canvas.SetLeft(e, c.mCenter.X - c.mRadius);
		// 			Canvas.SetTop(e, c.mCenter.Y - c.mRadius);
		// 		}
		// 		else
		// 		{
		// 			var e = new Ellipse();
		// 			e.Width = e.Height = c.mRadius * 2;
		// 			e.Fill = new SolidColorBrush(c.circleColor);
		// 			e.Stroke = Brushes.Black;
		// 			Canvas.SetLeft(e, c.mCenter.X - c.mRadius);
		// 			Canvas.SetTop(e, c.mCenter.Y - c.mRadius);
		// 			HostCanvas.Children.Add(e);
		// 		}
		// 	}
		// }
		//
		// var mainCircle = new Ellipse
		// {
		// 	Width = 100 * 2,
		// 	Height = 100 * 2,
		// 	Stroke = Brushes.Black,
		// 	StrokeThickness = 1
		// };
		// HostCanvas.Children.Add(mainCircle);
	}
}

