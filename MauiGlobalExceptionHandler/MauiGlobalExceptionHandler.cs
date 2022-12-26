namespace MauiGlobalExceptionHandler;


public static class MauiGlobalExceptionHandler
{
	public static MauiAppBuilder EnableGlobalExceptionHandler(this MauiAppBuilder builder, bool showDeveloperPage)
	{
		ShowDeveloperPage = showDeveloperPage;
		AppDomain.CurrentDomain.UnhandledException += HandleException;
		TaskScheduler.UnobservedTaskException += (sender, args) =>
		{
			args.SetObserved();
			HandleException(sender, new UnhandledExceptionEventArgs(args.Exception, false));
		};

#if IOS || MACCATALYST
		ObjCRuntime.Runtime.MarshalManagedException += (_, args) =>
		{
			args.ExceptionMode = ObjCRuntime.MarshalManagedExceptionMode.UnwindNativeCode;
		};
#elif ANDROID
		Java.Lang.Thread.DefaultUncaughtExceptionHandler = new UncaughtExceptionHandler();
		Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
		{
			HandleException(sender, new UnhandledExceptionEventArgs(args.Exception, true));
		};
#elif WINDOWS
		AppDomain.CurrentDomain.FirstChanceException += (_, args) =>
        {
            _lastFirstChanceException = args.Exception;
        };

		Microsoft.UI.Xaml.Application.Current.Dispatcher.UnhandledException += (s, e) =>
		{
			e.Handled = true;

            if (e.StackTrace is null)
            {
                e = _lastFirstChanceException;
            }

            HandleException(sender, new UnhandledExceptionEventArgs(e, true));
		};

		Microsoft.UI.Xaml.Application.Current.DispatcherUnhandledException += (s, e) =>
		{
			e.Handled = true;

            if (e.StackTrace is null)
            {
                e = _lastFirstChanceException;
            }

            HandleException(sender, new UnhandledExceptionEventArgs(e, true));
		};

        Microsoft.UI.Xaml.Application.Current.UnhandledException += (sender, args) =>
        {
            var exception = args.Exception;

            if (exception.StackTrace is null)
            {
                exception = _lastFirstChanceException;
            }

            HandleException(sender, new UnhandledExceptionEventArgs(exception, true));
        };
#endif

		return builder;
	}

	private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
	{
		throw new NotImplementedException();
	}

#if WINDOWS
    private static Exception _lastFirstChanceException;
#endif

	public static bool ShowDeveloperPage { get; set; }
	public static event UnhandledExceptionEventHandler? UnhandledException;

	static void HandleException(object? sender, UnhandledExceptionEventArgs eventArgs)
	{
		UnhandledException?.Invoke(sender ?? new object(), eventArgs);
		if (ShowDeveloperPage)
		{
			var developerPage = new ContentPage()
			{
				Content = new VerticalStackLayout()
				{
					Children =
					{
						new Label()
						{
							Text = eventArgs.IsTerminating.ToString()
						},
						new Label()
						{
							Text = eventArgs.ExceptionObject.ToString()
						}
					}
				}
			};
			Application.Current?.MainPage?.Navigation.PushModalAsync(developerPage);
		}
	}
}

#if ANDROID
class UncaughtExceptionHandler : Java.Lang.Object, Java.Lang.Thread.IUncaughtExceptionHandler
{
	public void UncaughtException(Thread t, Throwable e)
	{
	}
}
#endif