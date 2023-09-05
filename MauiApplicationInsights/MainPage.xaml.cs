namespace MauiApplicationInsights;

using Microsoft.Extensions.Logging;

public partial class MainPage : ContentPage
{
	int count;
	private readonly ILogger<MainPage> logger;

	public MainPage(ILogger<MainPage> logger)
	{
		this.logger = logger;
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		CounterBtn.Text = count == 1 ? $"Clicked {count} time" : $"Clicked {count} times";
		logger.LogInformation("Button Clicked {Count} times", count);
		SemanticScreenReader.Announce(CounterBtn.Text);
		try
		{
			if (count > 2)
			{
				throw new Exception("Count > 2");
			}
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "Something went wrong: {Count}", count);
		}

		if (count > 5)
		{
			throw new Exception("Count > 5");
		}
	}
}