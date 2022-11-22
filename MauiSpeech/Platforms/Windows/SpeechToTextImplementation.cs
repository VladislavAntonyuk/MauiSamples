using System.Globalization;

namespace MauiSpeech.Platforms;

using Windows.Media.SpeechRecognition;

public class SpeechToTextImplementation : ISpeechToText
{
	private SpeechRecognizer? speechRecognizer;
	private string? recognitionText;
	public Task<bool> RequestPermissions()
	{
		return Task.FromResult(true);
	}

	public async Task<string> Listen(CultureInfo culture, IProgress<string>? recognitionResult, CancellationToken cancellationToken)
	{
		recognitionText = string.Empty;
		speechRecognizer = new SpeechRecognizer();
		await speechRecognizer.CompileConstraintsAsync();

		var taskResult = new TaskCompletionSource<string>();
		speechRecognizer.ContinuousRecognitionSession.ResultGenerated += (s, e) =>
		{
			recognitionText += e.Result.Text;
			recognitionResult?.Report(e.Result.Text);
		};
		speechRecognizer.ContinuousRecognitionSession.Completed += (s, e) =>
		{
			switch (e.Status)
			{
				case SpeechRecognitionResultStatus.Success:
					taskResult.TrySetResult(recognitionText);
					break;
				case SpeechRecognitionResultStatus.UserCanceled:
					taskResult.TrySetCanceled();
					break;
				default:
					taskResult.TrySetException(new Exception(e.Status.ToString()));
					break;
			}
		};
		await speechRecognizer.ContinuousRecognitionSession.StartAsync();
		await using (cancellationToken.Register(async () =>
					 {
						 await StopRecording();
						 taskResult.TrySetCanceled();
					 }))
		{
			return await taskResult.Task;
		}
	}

	private async Task StopRecording()
	{
		try
		{
			await speechRecognizer?.ContinuousRecognitionSession.StopAsync();
		}
		catch
		{
			// ignored. Recording may be already stopped
		}
	}

	public async ValueTask DisposeAsync()
	{
		await StopRecording();
		speechRecognizer?.Dispose();
	}
}

