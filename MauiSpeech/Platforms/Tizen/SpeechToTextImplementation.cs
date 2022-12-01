using System.Globalization;

namespace MauiSpeech.Platforms;

public sealed class SpeechToTextImplementation : ISpeechToText
{
	public Task<bool> RequestPermissions()
	{
		return Task.FromResult(false);
	}

	public Task<string> Listen(CultureInfo culture, IProgress<string>? recognitionResult, CancellationToken cancellationToken)
	{
		return Task.FromResult("Not implemented");
	}

	public ValueTask DisposeAsync()
	{
		return ValueTask.CompletedTask;
	}
}

