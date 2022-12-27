namespace MauiBells;

using Plugin.Maui.Audio;

public sealed partial class MainPage : ContentPage, IDisposable
{
	private IAudioPlayer? audioPlayer;
	public MainPage()
	{
		InitializeComponent();
		if (Accelerometer.Default.IsSupported)
		{
			Accelerometer.Default.ShakeDetected += ShakeDetected;
			Accelerometer.Default.ReadingChanged += ReadingChanged;
			Accelerometer.Default.Start(SensorSpeed.UI);
		}

		Task.Factory.StartNew(async () =>
		{
			// "https://jesusful.com/wp-content/uploads/music/2020/09/Boney_M_-_Jingle_Bells_(Jesusful.com).mp3"
			var fileStream = await FileSystem.OpenAppPackageFileAsync("jingle_bells.mp3");

			audioPlayer = AudioManager.Current.CreatePlayer(fileStream);
			while (true)
			{
				var dateTime = DateTime.Now;
				var tasks = new[]
				{
					Flip(year, dateTime.Year),
					Flip(month, dateTime.Month),
					Flip(day, dateTime.Day),
					Flip(hour, dateTime.Hour),
					Flip(minute, dateTime.Minute),
					Flip(second, dateTime.Second)
				};
				await Task.WhenAll(tasks);
				if (lastShakeDetected + TimeSpan.FromSeconds(5) < DateTime.Now)
				{
					audioPlayer.Pause();
				}
			}
		});
	}

	private async void ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
	{
		await bell.RotateTo(e.Reading.Acceleration.X * 180 / Math.PI);
	}

	DateTime lastShakeDetected;
	private void ShakeDetected(object? sender, EventArgs e)
	{
		lastShakeDetected = DateTime.Now;
		if (audioPlayer is { IsPlaying: false })
		{
			audioPlayer.Play();
		}
	}

	public void Dispose()
	{
		Accelerometer.Default.ShakeDetected -= ShakeDetected;
		Accelerometer.Default.ReadingChanged -= ReadingChanged;
	}

	async Task Flip(Label label, int text)
	{
		var textString = text.ToString();
		if (label.Text != textString)
		{
			await label.RotateXTo(90, 500);
			await Dispatcher.DispatchAsync(() =>
			{
				label.Text = textString;
			});
			await label.RotateXTo(0, 500);
		}
	}
}


