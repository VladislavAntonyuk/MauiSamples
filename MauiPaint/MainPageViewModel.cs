namespace MauiPaint;
using System.Collections.ObjectModel;
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
    private static List<IFigure> _figures = new ();

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
    private Action<ICanvas, RectF>? drawAction = (canvas, rect) =>
    {
	    foreach (var figure in _figures)
	    {
		    figure.Draw(canvas, rect);
	    }
    };

    [ICommand]
	void Quit()
	{
		Application.Current?.Quit();
	}

	[ICommand]
	void Help()
	{
		var help = new Help();
		Application.Current?.MainPage?.ShowPopup(help);
	}

	[ICommand]
	Task About()
	{
		return Launcher.OpenAsync("https://vladislavantonyuk.azurewebsites.net");
	}

	[ICommand]
	void ToggleTheme()
	{
		if (Application.Current is not null)
		{
			Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
		}
	}

	[ICommand]
	void New()
	{
		Lines.Clear();
		_figures.Clear();
		Background = Brush.White;
		LineColor = Colors.Black;
		LineWidth = 5;
	}
	
	[ICommand]
	void SetLineColor(Color color)
	{
		LineColor = color;
	}

	[ICommand]
	void SetEraser()
	{
		LineColor = Background is SolidColorBrush solidColorBrush? solidColorBrush.Color : Colors.White;
	}

	[ICommand]
	async Task AddFigure(string figureName)
	{
		var figure = await FigureFactory.CreateFigure(figureName);
		_figures.Add(figure);
	}

	[ICommand]
	void SetBackground(Color color)
	{
		Background = color;
	}

	[ICommand]
	async Task Open(CancellationToken cancellationToken)
	{
		await using var stream = await dialogService.OpenFileDialog(cancellationToken);
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

	[ICommand]
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

	[ICommand]
	async Task SaveImage(CancellationToken cancellationToken)
	{
		await using var stream = await DrawingView.GetImageStream(
			lines,
			new Size(100, 100),
			Background is SolidColorBrush solidColorBrush ? solidColorBrush.Color : Colors.White);
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