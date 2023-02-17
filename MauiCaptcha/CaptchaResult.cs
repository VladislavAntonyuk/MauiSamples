namespace MauiCaptcha;

using System.Text.Json.Serialization;

public class CaptchaResult
{
	[JsonPropertyName("success")]
	public bool Success { get; set; }

	[JsonPropertyName("challenge_ts")]
	public DateTime ChallengeTs { get; set; }

	[JsonPropertyName("apk_package_name")]
	public string? ApkPackageName { get; set; }

	[JsonPropertyName("error-codes")]
	public List<object>? ErrorCodes { get; set; }
}