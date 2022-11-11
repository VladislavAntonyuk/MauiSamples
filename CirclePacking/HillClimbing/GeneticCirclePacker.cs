namespace CirclePacking.HillClimbing;

using System.Numerics;

public class MyPoint
{
	public MyPoint(float x, float y)
	{
		X = x;
		Y = y;
	}

	public float X { get; set; }
	public float Y { get; set; }
	public double RToCircle { get; set; }
	public Dictionary<int, double> RToPoints { get; set; } = new Dictionary<int, double>();
}
public class Pair
{
	public List<MyPoint> Points { get; set; } = new List<MyPoint>();
	public double Fitness
	{
		get
		{
			double f = double.MaxValue;
			foreach (var p in Points)
			{
				var pf = Math.Min(p.RToCircle, p.RToPoints.Select(v => v.Value).Min());
				if (pf < f)
					f = pf;
			}
			if (f == double.MaxValue) f = 0;
			return f;
		}
	}

	public override string ToString()
	{
		return $"{Fitness}";
	}
}
public class GeneticCirclePacker : ICirclePacker
{
	public float eps = 0.0001f;

	private readonly double radius;

	/// <summary>
	/// </summary>
	private bool min;

	/// <summary>
	///     Generates a number of Packing circles in the constructor.
	///     Random distribution is linear
	/// </summary>
	public GeneticCirclePacker(ICanvas hostCanvas, double radius, int pNumCircles)
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

						AB = AB * (float)(r - Math.Sqrt(d)) * 0.5f;

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

