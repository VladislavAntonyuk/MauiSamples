namespace MauiMaps;

using Microsoft.Maui.Controls.Maps;

public class CustomPin : Pin {
  public static readonly BindableProperty ImageSourceProperty =
      BindableProperty.Create(nameof(ImageSource), typeof(ImageSource),
                              typeof(CustomPin),
                              propertyChanged: OnImageSourceChanged);

  public ImageSource? ImageSource {
    get => (ImageSource?)GetValue(ImageSourceProperty);
    set => SetValue(ImageSourceProperty, value);
  }

  public Microsoft.Maui.Maps.IMap? Map { get; set; }

  static async void OnImageSourceChanged(BindableObject bindable,
                                         object oldValue, object newValue) {
    var control = (CustomPin)bindable;
    var imageSource = control.ImageSource;

    if (control.Handler?.PlatformView is null) {
      // Workaround for when this executes the Handler and PlatformView is null
      control.HandlerChanged += OnHandlerChanged;
      return;
    }

    if (imageSource is not null) {
#if ANDROID || IOS || MACCATALYST
      await control.AddAnnotation();
#else
      await Task.CompletedTask;
#endif
    } else {
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
}