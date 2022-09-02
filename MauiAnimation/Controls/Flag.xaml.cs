namespace MauiAnimation.Controls;

using Microsoft.Maui.Controls.Shapes;

public partial class Flag : ContentView
{
	public Flag()
	{
		InitializeComponent();
		var flag1 = new Animation(v =>
		{
			Blue1.Clip = Yellow1.Clip = new PathGeometry(new PathFigureCollection()
			{
				new ()
				{
					Segments = new PathSegmentCollection()
					{
						new QuadraticBezierSegment(
							new Point(100,v*100),
							new Point(200,50))
					},
					StartPoint = new Point(0,50)
				}
			});
			Blue1.Fill = v > 0.5 ? Brush.LightBlue : new SolidColorBrush(Color.FromArgb("#0057B8"));
			Blue2.Fill = v < 0.5 ? Brush.LightBlue : new SolidColorBrush(Color.FromArgb("#0057B8"));
			Yellow1.Fill = v < 0.5 ? Brush.LightBlue : new SolidColorBrush(Color.FromArgb("#FFD700"));
			Yellow2.Fill = v > 0.5 ? Brush.LightBlue : new SolidColorBrush(Color.FromArgb("#FFD700"));
			Blue2.Clip = Yellow2.Clip = new PathGeometry(new PathFigureCollection()
			{
				new ()
				{
					Segments = new PathSegmentCollection()
					{
						new QuadraticBezierSegment(
							new Point(300,-v*100+100),
							new Point(400,50))
					},
					StartPoint = new Point(200,50)
				}
			});
		});
		var flag2 = new Animation(v =>
		{
			Blue1.Clip = Yellow1.Clip = new PathGeometry(new PathFigureCollection()
			{
				new ()
				{
					Segments = new PathSegmentCollection()
					{
						new QuadraticBezierSegment(
							new Point(100,v*100),
							new Point(200,50))
					},
					StartPoint = new Point(0,50)
				}
			});
			Blue1.Fill = v > 0.5 ? Brush.LightBlue : new SolidColorBrush(Color.FromArgb("#0057B8"));
			Blue2.Fill = v < 0.5 ? Brush.LightBlue : new SolidColorBrush(Color.FromArgb("#0057B8"));
			Yellow1.Fill = v < 0.5 ? Brush.LightBlue : new SolidColorBrush(Color.FromArgb("#FFD700"));
			Yellow2.Fill = v > 0.5 ? Brush.LightBlue : new SolidColorBrush(Color.FromArgb("#FFD700"));
			Blue2.Clip = Yellow2.Clip = new PathGeometry(new PathFigureCollection()
			{
				new ()
				{
					Segments = new PathSegmentCollection()
					{
						new QuadraticBezierSegment(
							new Point(300,-v*100+100),
							new Point(400,50))
					},
					StartPoint = new Point(200,50)
				}
			});
		}, 1, 0);
		RunFlag(flag1, flag2);
	}

	void RunFlag(Animation fade, Animation reverseFade)
	{
		fade.Commit(this, "Flag", length: 3000, finished: (s, t) => RunFlag(reverseFade, fade));
	}
}