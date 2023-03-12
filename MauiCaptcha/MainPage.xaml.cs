namespace MauiCaptcha;

using System.Net.Http.Json;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Platform;

public partial class MainPage : ContentPage
{
	private const string AndroidSiteKey = "";
	private const string AndroidSecretKey = "";
	private const string WebSiteKey = "6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI";
	private const string WebSecretKey = "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe";

	public MainPage()
	{
		InitializeComponent();
	}

	async void OnAndroidCaptchaVerificationClicked(object sender, EventArgs e)
	{
#if ANDROID
		var api = Android.Gms.SafetyNet.SafetyNetClass.GetClient(Platform.CurrentActivity);
		var response = await api.VerifyWithRecaptchaAsync(AndroidSiteKey);
		if (response != null && !string.IsNullOrEmpty(response.TokenResult))
		{
			var captchaResponse = await ValidateCaptcha(response.TokenResult, AndroidSecretKey);
			if (captchaResponse is null || !captchaResponse.Success)
			{
				await Toast.Make($"Invalid captcha: {string.Join(",", captchaResponse?.ErrorCodes ?? Enumerable.Empty<object>())}", ToastDuration.Long).Show();
				return;
			}

			if (Platform.CurrentActivity!.PackageName != captchaResponse.ApkPackageName)
			{
				await Toast.Make($"Package Names do not match: {captchaResponse.ApkPackageName}", ToastDuration.Long).Show();
			}
			else
			{
				await Toast.Make("Success", ToastDuration.Long).Show();
			}
		}
		else
		{
			await Toast.Make("Failed", ToastDuration.Long).Show();
		}
#else
		await Toast.Make("This button works only on Android", ToastDuration.Long).Show();
#endif
	}

	private async void BlazorWebView_OnUrlLoading(object? sender, UrlLoadingEventArgs e)
	{
		e.UrlLoadingStrategy = UrlLoadingStrategy.OpenInWebView;
		var query = System.Web.HttpUtility.ParseQueryString(e.Url.Query);
		var token = query.Get("token");
		if (!string.IsNullOrEmpty(token))
		{
			var captchaResponse = await ValidateCaptcha(token, WebSecretKey);
			if (captchaResponse is null || !captchaResponse.Success)
			{
				await Toast.Make($"Invalid captcha: {string.Join(",", captchaResponse?.ErrorCodes ?? Enumerable.Empty<object>())}", ToastDuration.Long).Show();
				return;
			}

			await Toast.Make("Success", ToastDuration.Long).Show();
		}
	}

	static async Task<CaptchaResult?> ValidateCaptcha(string token, string secretKey)
	{
		using var client = new HttpClient();
		var parameters = new Dictionary<string, string>
		{
			{ "secret", secretKey },
			{ "response", token }
		};
		var content = new FormUrlEncodedContent(parameters);
		var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);

		if (!response.IsSuccessStatusCode)
		{
			return null;
		}

		var responseContent = await response.Content.ReadFromJsonAsync<CaptchaResult>();
		return responseContent;

	}
}