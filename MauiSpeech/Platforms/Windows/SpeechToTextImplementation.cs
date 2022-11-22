using System;
using System.Globalization;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using Speech;

namespace MauiSpeech.Platforms;

public class SpeechToTextImplementation : ISpeechToText
{
	public Task<string> Listen(CultureInfo culture, IProgress<string>? recognitionResult)
	{
		return Task.FromResult("Not implemented");
	}
}

