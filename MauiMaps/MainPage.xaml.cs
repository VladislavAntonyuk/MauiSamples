namespace MauiMaps;

using MapKit;
using Microsoft.Maui.Controls.Maps;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		var customPin = new CustomPin()
		{
			Location = new Location(10, 10),
			ImageSource = ImageSource.FromUri(new Uri("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png")),
		};
		MyMap.Pins.Add(customPin);
	}
}

public class CustomPin : Pin
{
	public static readonly BindableProperty ImageSourceProperty =
		BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(CustomPin), propertyChanged:OnImageSourceChanged);

	public ImageSource? ImageSource
	{
		get => (ImageSource?)GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
	}

	static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomPin)bindable;
        var imageSource = control.ImageSource;

        if (control.Handler?.PlatformView is null)
        {
            // Workaround for when this executes the Handler and PlatformView is null
            control.HandlerChanged += OnHandlerChanged;
            return;
        }

        if (imageSource is not null)
        {
#if ANDROID
            // Note the use of Android.Widget.ImageView which is an Android-specific API
            // You can find the Android implementation of `ApplyColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.android.cs#L9-L12
            ImageExtensions.ApplyColor((Android.Widget.ImageView)control.Handler.PlatformView, tintColor);
#elif IOS
	        (control.Parent as  Microsoft.Maui.Maps.IMap)?.AddConferenceAnnotation();
#endif
        }
        else
        {
#if ANDROID

#elif IOS

#endif
        }

        void OnHandlerChanged(object? s, EventArgs e)
        {
            OnImageSourceChanged(control, oldValue, newValue);
            control.HandlerChanged -= OnHandlerChanged;
        }
}