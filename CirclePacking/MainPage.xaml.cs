namespace CirclePacking;

using System.Numerics;
using HillClimbing;
using Microsoft.Maui.Controls.Shapes;
using Color = Microsoft.Maui.Graphics.Color;
using Point = Microsoft.Maui.Graphics.Point;
using RectF = Microsoft.Maui.Graphics.RectF;
using System.Windows;
public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		Packing.Drawable = new GeneticPackingDrawable();
	}
}

public class GeneticPackingDrawable : IDrawable
{
	private RandomPointGenerator pointGen = new RandomPointGenerator();
	ICanvas canvas = null!;

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		this.canvas = canvas;
		var cCount = Convert.ToInt32("5");
		if (cCount < 2)
		{
			return;
		}

		int populCount = Convert.ToInt32("100");
		int childCount = Convert.ToInt32("5");
		int iter = Convert.ToInt32("1000");
		double eps = 0.01;

		var radius = Convert.ToInt32(100 / 2);

		//canvas.Children.Clear();
		var mainCircle = new Ellipse
		{
			WidthRequest = radius * 2,
			HeightRequest = radius * 2,
			Stroke = Brush.Black,
			StrokeThickness = 1
		};
		canvas.DrawEllipse(0, 0, radius, radius);
		//CircleCanvas.Children.Add(mainCircle);

		// Genetic Algorithm
		var population = new List<Pair>();
		//if (FastSolution.IsChecked == true)
		{
			population.AddRange(GeneratePopulation2(populCount, radius, cCount));
		}
		// else if (FastSolution.IsChecked == null)
		// {
		//     population.AddRange(GeneratePopulation(populCount / 2, radius, cCount));
		//     population.Add(GeneratePair(radius, cCount));
		//     population.AddRange(GeneratePopulation2(populCount / 2 - 1, radius, cCount));
		// }
		// else
		// {
		//     population.AddRange(GeneratePopulation(populCount, radius, cCount));
		// }

		var curIter = 0;
		List<double> fitnessChange = new List<double>();
		List<double> fitnessChange2 = new List<double>();
		do
		{
			curIter++;
			//Fitness
			population = OrderFitness(population, radius);
			//Parents
			double fitnessTotal = 0;
			foreach (var item in population)
			{
				fitnessTotal += item.Fitness;
			}

			var p1 = population[RouletteSelection(population, fitnessTotal)];
			//var p2 = population[RouletteSelection(population, fitnessTotal)];
			//var p1 = population[rouletteSelect(population)];
			var p2 = population[rouletteSelect(population)];

			var tempPoints = new List<Pair>();
			for (int i = 0; i < childCount; i++)
			{
				// CrossOver
				var child = BlxA(p1, p2);
				// Mutation
				child = pointGen.Mutation(child, eps);
				tempPoints.Add(child);
			}

			tempPoints.AddRange(population);

			// Fitness
			population = OrderFitness(tempPoints, radius);
			// Remove the weakness
			for (int i = 0; i < childCount; i++)
			{
				population.Remove(population.Last());
			}

			fitnessChange.Add(population.First().Fitness);
			fitnessChange2.Add(population.Last().Fitness);
			System.IO.File.WriteAllText(
				"1.txt", $"{curIter}\t{population.First().Fitness}\t{population.Last().Fitness}");
		} while (curIter < iter && Math.Abs(population.First().Fitness - population.Last().Fitness) > eps);

		var bestRadius = population.First().Fitness;
		// Title = curIter + " " + bestRadius + " " + population.Last().Fitness;
		// var ri = 0;
		// ((LineSeries)mcChart.Series[0]).ItemsSource = fitnessChange.Select(r => new KeyValuePair<int, double>(ri++, r));
		// ri = 0;
		// ((LineSeries)mcChart.Series[1]).ItemsSource = fitnessChange2.Select(r => new KeyValuePair<int, double>(ri++, r));
		//
		// foreach (var point in population.First().Points)
		// {
		//     var mainCircle3 = new Ellipse
		//     {
		//         Width = bestRadius * 2,
		//         Height = bestRadius * 2,
		//         Stroke = Brushes.Black,
		//         StrokeThickness = 1
		//     };
		//
		//     Canvas.SetLeft(mainCircle3, point.X + radius - bestRadius);
		//     Canvas.SetTop(mainCircle3, point.Y + radius - bestRadius);
		//     CircleCanvas.Children.Add(mainCircle3);
		//     //Text(point.X + radius, point.Y + radius, point.X + radius + " " + point.Y + radius);
		//     //       g.DrawEllipse(p, point.X + radius, point.Y+radius, 2, 2);
		// }
	}

	public float GetRandomNumber(double minimum, double maximum)
	{
		Random random = new Random();
		return (float)(random.NextSingle() * (maximum - minimum) + minimum);
	}

	private Pair BlxA(Pair p1, Pair p2)
	{
		var points = new List<MyPoint>();
		for (int i = 0; i < p1.Points.Count; i++)
		{
			MyPoint po1 = p1.Points[i];
			MyPoint po2 = p2.Points[i];

			var gamma = GetRandomNumber(-0.5, 1.5);
			var childV = gamma * new Vector2(po1.X, po1.Y) + (1 - gamma) * new Vector2(po2.X, po2.Y);
			points.Add(new MyPoint(childV.X, childV.Y));
		}

		return new Pair()
		{
			Points = points
		};
	}

	private List<Pair> OrderFitness(List<Pair> population, int radius)
	{
		foreach (var item in population)
		{
			var myPoints = new List<MyPoint>();
			for (var i = 0; i < item.Points.Count; i++)
			{
				var myPoint = new MyPoint(item.Points[i].X, item.Points[i].Y);
				myPoint.RToCircle = (radius - Math.Sqrt(Math.Pow(item.Points[i].X, 2) + Math.Pow(item.Points[i].Y, 2)));
				for (var j = 0; j < item.Points.Count; j++)
				{
					if (i == j)
					{
						continue;
					}

					var d = 0.5 *
					        Math.Sqrt(Math.Pow(item.Points[i].X - item.Points[j].X, 2) +
					                  Math.Pow(item.Points[i].Y - item.Points[j].Y, 2));
					myPoint.RToPoints.Add(j, d);
				}

				myPoints.Add(myPoint);
			}

			item.Points = myPoints;
		}


		return population.OrderByDescending(p => p.Fitness).ToList();
	}

	// private void Text(double x, double y, string text)
	// {
	//     var textBlock = new TextBlock();
	//     textBlock.Text = text;
	//     textBlock.Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150));
	//     Canvas.SetLeft(textBlock, x);
	//     Canvas.SetTop(textBlock, y);
	//     CircleCanvas.Children.Add(textBlock);
	// }

	private List<Pair> GeneratePopulation(int populationCount, int radius, int cCount)
	{
		var population = new List<Pair>();

		for (int i = 0; i < populationCount; i++)
		{
			var pair = new Pair();
			pair.Points = pointGen.GetPointsInACircle2(radius, cCount);
			population.Add(pair);
		}

		return population;
	}

	private Pair GeneratePair(int radius, int cCount)
	{
		var pair = new Pair();
		var cp = new GeneticCirclePacker(canvas, radius, cCount);
		cp.Iterate();
		var randomPoints = cp.AllCircles.Select(t => new PointF(t.mCenter.X - radius, t.mCenter.Y - radius)).ToList();

		foreach (var item in randomPoints)
		{
			pair.Points.Add(new MyPoint(item.X, item.Y));
		}

		return pair;
	}

	private List<Pair> GeneratePopulation2(int populationCount, int radius, int cCount)
	{
		var population = new List<Pair>();

		for (int i = 0; i < populationCount; i++)
		{
			var pair = new Pair();
			var cp = new GeneticCirclePacker(canvas, radius, cCount);
			cp.eps = new Random().NextSingle();
			cp.Iterate();
			var randomPoints = cp.AllCircles.Select(t => new PointF(t.mCenter.X - radius, t.mCenter.Y - radius))
			                     .ToList();

			foreach (var item in randomPoints)
			{
				pair.Points.Add(new MyPoint(item.X, item.Y));
			}

			population.Add(pair);
		}

		return population;
	}

	/// <summary>
	/// After ranking all the genomes by fitness, use a 'roulette wheel' selection
	/// method.  This allocates a large probability of selection to those with the
	/// highest fitness.
	/// </summary>
	/// <returns>Random individual biased towards highest fitness</returns>
	private int RouletteSelection(List<Pair> population, double totalFitness)
	{
		double randomFitness = new Random().NextDouble() * totalFitness;
		int idx = -1;
		int mid;
		int first = 0;
		int last = population.Count() - 1;
		mid = (last - first) / 2;
		//  ArrayList's BinarySearch is for exact values only
		//  so do this by hand.
		while (idx == -1 && first <= last)
		{
			if (randomFitness < population[mid].Fitness)
			{
				last = mid;
			}
			else if (randomFitness > population[mid].Fitness)
			{
				first = mid;
			}

			mid = (first + last) / 2;
			//  lies between i and i+1
			if ((last - first) == 1)
				idx = last;
		}

		return idx;
	}

	// Returns the selected index based on the weights(probabilities)
	public int rouletteSelect(List<Pair> population)
	{
		var weight = new List<double>();
		foreach (var item in population)
		{
			weight.Add(item.Fitness);
		}

		// calculate the total weight
		double weight_sum = 0;
		for (int i = 0; i < weight.Count; i++)
		{
			weight_sum += weight[i];
		}

		// get a random value
		double value = new Random().NextDouble() * weight_sum;
		// locate the random value based on the weights
		for (int i = 0; i < weight.Count; i++)
		{
			value -= weight[i];
			if (value < 0)
				return i;
		}

		// when rounding errors occur, we return the last item's index
		return weight.Count - 1;
	}
}