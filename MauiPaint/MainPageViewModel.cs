﻿namespace MauiPaint;

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Threading;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Figures;
using Serializer;
using Services;

public partial class MainPageViewModel : ObservableObject
{
	private readonly ISerializerService serializerService;
	private readonly IDialogService dialogService;
	private static List<IFigure> _figures = new();

	public MainPageViewModel(ISerializerService serializerService, IDialogService dialogService)
	{
		this.serializerService = serializerService;
		this.dialogService = dialogService;
	}

	[ObservableProperty]
	private ObservableCollection<IDrawingLine> lines = new();
	[ObservableProperty]
	private Brush background = Brush.White;
	[ObservableProperty]
	private Color lineColor = Colors.Black;
	[ObservableProperty]
	private float lineWidth = 5;
	[ObservableProperty]
	private float canvasWidth;
	[ObservableProperty]
	private float canvasHeight;

	[ObservableProperty]
	private Action<ICanvas, RectF>? drawAction = (canvas, rect) =>
	{
		foreach (var figure in _figures)
		{
			figure.Draw(canvas, rect);
		}
	};

	[RelayCommand]
	void Quit()
	{
		Application.Current?.Quit();
	}

	[RelayCommand]
	void Help()
	{
		var help = new Help();
		Application.Current?.MainPage?.ShowPopup(help);
	}

	[RelayCommand]
	Task About()
	{
		return Launcher.OpenAsync("https://vladislavantonyuk.azurewebsites.net");
	}

	[RelayCommand]
	void ToggleTheme()
	{
		if (Application.Current is not null)
		{
			Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
		}
	}

	[RelayCommand]
	void New()
	{
		Lines.Clear();
		_figures.Clear();
		Background = Brush.White;
		LineColor = Colors.Black;
		LineWidth = 5;
	}

	[RelayCommand]
	void SetLineColor(Color color)
	{
		LineColor = color;
	}

	[RelayCommand]
	void SetEraser()
	{
		LineColor = Background is SolidColorBrush solidColorBrush ? solidColorBrush.Color : Colors.White;
	}

	[RelayCommand]
	async Task AddFigure(string figureName)
	{
		var figure = await FigureFactory.CreateFigure(figureName);
		_figures.Add(figure);
	}

	[RelayCommand]
	void SetBackground(Color color)
	{
		Background = color;
	}

	[RelayCommand]
	void Rotate(double angle)
	{
		var oldLines = Lines.ToImmutableList();
		foreach (var line in oldLines)
		{
			var points = line.Points.ToImmutableList();
			line.Points.Clear();
			foreach (var point in points)
			{
				line.Points.Add(RotatePoint(point, new PointF(canvasWidth / 2, canvasHeight / 2), angle));
			}
		}

		Lines = oldLines.ToObservableCollection();

		static PointF RotatePoint(PointF pointToRotate, PointF centerPoint, double angleInDegrees)
		{
			double angleInRadians = angleInDegrees * (Math.PI / 180);
			double cosTheta = Math.Cos(angleInRadians);
			double sinTheta = Math.Sin(angleInRadians);
			return new PointF
			{
				X =
					(float)
					(cosTheta * (pointToRotate.X - centerPoint.X) -
						sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
				Y =
					(float)
					(sinTheta * (pointToRotate.X - centerPoint.X) +
					 cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
			};
		}
	}

	[RelayCommand]
	async Task PasteFromClipboard(CancellationToken cancellationToken)
	{
#if IOS || MACCATALYST
		var image = UIKit.UIPasteboard.General.Image;
		if (image is null)
		{
			await Toast.Make("Clipboard doesn't contain images").Show(cancellationToken);
		}
		else
		{
			var imageFigure = new ImageFigure();
			var stream = new MemoryStream();
			image.AsPNG().AsStream().CopyTo(stream);
			imageFigure.ImageStream = stream;
			imageFigure.Width = (int)image.Size.Width;
			imageFigure.Height = (int)image.Size.Height;
			_figures.Add(imageFigure);
		}
#else
		await Toast.Make("Not supported").Show(cancellationToken);
#endif
	}

	[RelayCommand]
	async Task Open(CancellationToken cancellationToken)
	{
		await using var stream = await dialogService.OpenFileDialog(cancellationToken);
		if (stream == Stream.Null)
		{
			return;
		}

		try
		{
			var projectState = await serializerService.Deserialize<ProjectState>(stream, cancellationToken);
			if (projectState is null)
			{
				return;
			}

			Background = projectState.Background;
			Lines = projectState.Lines.ToObservableCollection();
			_figures = projectState.Figures.ToList();
			LineWidth = projectState.LineWidth;
			LineColor = projectState.LineColor;
		}
		catch
		{
			await Toast.Make("Invalid file").Show(cancellationToken);
		}

	}

	[RelayCommand]
	async Task Save(CancellationToken cancellationToken)
	{
		var projectState = new ProjectState
		{
			Figures = _figures.ToArray(),
			Lines = Lines.ToArray(),
			LineColor = LineColor,
			LineWidth = LineWidth,
			Background = Background
		};
		await using var stream = await serializerService.Serialize(projectState, cancellationToken);
		await SaveToFile(stream, ".json", cancellationToken);
	}

	[RelayCommand]
	async Task SaveImage(CancellationToken cancellationToken)
	{
		await using var stream = await DrawingView.GetImageStream(
			lines,
			new Size(1000, 1000),
			Background);
		await SaveToFile(stream, ".png", cancellationToken);
	}

	async Task SaveToFile(Stream stream, string fileExtension, CancellationToken cancellationToken)
	{
		var isSaved = await dialogService.SaveFileDialog(stream, fileExtension, cancellationToken);
		if (isSaved)
		{
			await Toast.Make("File is saved", ToastDuration.Long).Show(cancellationToken);
		}
	}
}