using System;
using System.Globalization;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using Speech;

namespace MauiSpeech.Platforms;

public class SpeechToTextImplementation : ISpeechToText, IDisposable
{
	private AVAudioEngine? audioEngine;
	private SFSpeechAudioBufferRecognitionRequest? liveSpeechRequest;
	private SFSpeechRecognizer? speechRecognizer;
	private SFSpeechRecognitionTask? recognitionTask;

	public void Dispose()
	{
		audioEngine?.Dispose();
		speechRecognizer?.Dispose();
		liveSpeechRequest?.Dispose();
		recognitionTask?.Dispose();
	}

	public async Task<string> Listen(CultureInfo culture, IProgress<string>? recognitionResult, CancellationToken cancellationToken)
	{
		speechRecognizer = culture is null
				? new SFSpeechRecognizer()
				: new SFSpeechRecognizer(NSLocale.FromLocaleIdentifier(culture.Name));

		if (!speechRecognizer.Available)
		{
			throw new ArgumentException("Speech recognizer is not available");
		}

		if (SFSpeechRecognizer.AuthorizationStatus != SFSpeechRecognizerAuthorizationStatus.Authorized)
		{
			throw new Exception("Permission denied");
		}

		audioEngine = new AVAudioEngine();
		liveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
		var node = audioEngine.InputNode;
		var recordingFormat = node.GetBusOutputFormat(0);
		node.InstallTapOnBus(0, 1024, recordingFormat, (AVAudioPcmBuffer buffer, AVAudioTime when) =>
		{
			liveSpeechRequest.Append(buffer);
		});

		audioEngine.Prepare();
		audioEngine.StartAndReturnError(out var error);

		if (error is not null)
		{
			throw new ArgumentException("Error starting audio engine - " + error.LocalizedDescription);
		}

		var currentIndex = 0;
		var taskResult = new TaskCompletionSource<string>();
		recognitionTask = speechRecognizer.GetRecognitionTask(liveSpeechRequest, (result, err) =>
		{
			if (err != null)
			{
				StopRecording();
				taskResult.TrySetException(new Exception(err.LocalizedDescription));
			}
			else
			{
				if (result.Final)
				{
					currentIndex = 0;
					StopRecording();
					taskResult.TrySetResult(result.BestTranscription.FormattedString);
				}
				else
				{
					for (var i = currentIndex; i < result.BestTranscription.Segments.Length; i++)
					{
						var s = result.BestTranscription.Segments[i].Substring;
						currentIndex++;
						recognitionResult?.Report(s);
					}
				}
			}
		});

		using (cancellationToken.Register(() =>
		{
			StopRecording();
			taskResult.TrySetCanceled();
		}))
		{
			return await taskResult.Task;
		}
	}

	void StopRecording()
	{
		audioEngine?.InputNode?.RemoveTapOnBus(0);
		audioEngine?.Stop();
		liveSpeechRequest?.EndAudio();
		recognitionTask?.Cancel();
	}

	public Task<bool> RequestPermissions()
	{
		var taskResult = new TaskCompletionSource<bool>();
		SFSpeechRecognizer.RequestAuthorization(status =>
		{
			taskResult.SetResult(status == SFSpeechRecognizerAuthorizationStatus.Authorized);
		});

		return taskResult.Task;
	}
}

