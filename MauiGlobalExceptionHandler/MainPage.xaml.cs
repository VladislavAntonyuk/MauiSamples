namespace MauiGlobalExceptionHandler;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void ThrowException(object sender, EventArgs e)
	{
#if ANDROID
		//throw new Java.Lang.RuntimeException("Opps");
		Java.Util.Concurrent.Executors.NewSingleThreadExecutor()?
			.Execute(new Java.Lang.Runnable(() =>
			{
				throw new Exception("ex");
			}));
#endif
		throw new NotImplementedException("This exception is handled by MauiGlobalExceptionHandler");
	}
}