using System;
using System.Globalization;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;

namespace MauiSpeech.Platforms;

public class SpeechToTextImplementation : ISpeechToText
{
	public Task<string> Listen(CultureInfo culture, IProgress<string>? recognitionResult)
	{
		var taskResult = new TaskCompletionSource<string>();
		using var listener = new SpeechRecognitionListener
		{
			Error = ex => taskResult.SetException(new Exception("Failure in speech engine - " + ex)),
			PartialResults = sentence =>
			{
				recognitionResult?.Report(sentence);
			},
			Results = sentence =>
			{
				taskResult.SetResult(sentence);
			}
		};
		using var speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(Android.App.Application.Context);
		if (speechRecognizer is null)
		{
			throw new ArgumentException("Speech recognizer is not available");
		}

		speechRecognizer.SetRecognitionListener(listener);
		speechRecognizer.StartListening(this.CreateSpeechIntent(true));
		return taskResult.Task;
	}

	private Intent CreateSpeechIntent(bool partialResults)
	{
		var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
		intent.PutExtra(RecognizerIntent.ExtraLanguagePreference, Java.Util.Locale.Default);
		intent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
		intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
		intent.PutExtra(RecognizerIntent.ExtraCallingPackage, Android.App.Application.Context.PackageName);
		//intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
		//intent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
		//intent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
		//intent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
		intent.PutExtra(RecognizerIntent.ExtraPartialResults, partialResults);

		return intent;
	}



	public Task<bool> RequestPermissions()
	{
		var taskResult = new TaskCompletionSource<bool>();
		taskResult.SetResult(true);

		return taskResult.Task;
	}
}

public class SpeechRecognitionListener : Java.Lang.Object, IRecognitionListener
{
	public Action<SpeechRecognizerError>? Error { get; set; }
	public Action<string>? PartialResults { get; set; }
	public Action<string>? Results { get; set; }
	public void OnBeginningOfSpeech()
	{
		
	}

	public void OnBufferReceived(byte[]? buffer)
	{
	}

	public void OnEndOfSpeech()
	{
	}

	public void OnError([GeneratedEnum] SpeechRecognizerError error)
	{
		Error?.Invoke(error);
	}

	public void OnEvent(int eventType, Bundle? @params)
	{
	}

	public void OnPartialResults(Bundle? partialResults)
	{
		SendResults(partialResults, PartialResults);
	}

	public void OnReadyForSpeech(Bundle? @params)
	{
	}

	public void OnResults(Bundle? results)
	{
		SendResults(results, Results);
	}

	public void OnRmsChanged(float rmsdB)
	{
	}

	void SendResults(Bundle? bundle, Action<string>? action)
	{
		var matches = bundle?.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
		if (matches == null || matches.Count == 0)
		{
			return;
		}

		action?.Invoke(matches.First());
	}
}
