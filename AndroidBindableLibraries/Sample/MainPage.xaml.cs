namespace Sample;

using Android.Views;
using Com.Pedro.Rtplibrary.Rtmp;
using Com.Skydoves.Balloon;
using Microsoft.Maui.Platform;
using RtpLibrary;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		var balloon = new Balloon.Builder(Platform.AppContext)
					  .SetText("Edit your profile here!")
					  .SetTextSize(15f)
					  .SetArrowPositionRules(ArrowPositionRules.AlignAnchor)
					  .SetArrowSize(10)
					  .SetArrowPosition(0.5f)
					  .SetPadding(12)
					  .SetCornerRadius(8f)
					  .SetBalloonAnimation(BalloonAnimation.Elastic)
					  .SetOnBalloonDismissListener(new BallonDismissListener(() =>
					  {
						  var balloon2 = new Balloon.Builder(Platform.AppContext)
										 .SetWidthRatio(1.0f)
										 .SetHeight(BalloonSizeSpec.Wrap)
										 .SetLayout(new BallonContent().ToPlatform(Handler.MauiContext))
										 .SetArrowPositionRules(ArrowPositionRules.AlignAnchor)
										 .SetArrowSize(10)
										 .SetArrowPosition(0.5f)
										 .SetPadding(12)
										 .SetCornerRadius(8f)
										 .SetBalloonAnimation(BalloonAnimation.Fade)
										 .Build();
						  balloon2.ShowAtCenter(MauiLabel.ToPlatform(MauiLabel.Handler.MauiContext));
					  }))
					  .Build();
		balloon.ShowAtCenter(CounterBtn.ToPlatform(CounterBtn.Handler.MauiContext));

		var isGranted = await Permissions.RequestAsync<Permissions.Camera>() == PermissionStatus.Granted
					 && await Permissions.RequestAsync<Permissions.Microphone>() == PermissionStatus.Granted;
		if (!isGranted)
		{
			return;
		}

		var surface = MauiSurfaceView.ToPlatform(Handler.MauiContext) as SurfaceView;
		var camera = new RtmpCamera1(surface, new DefaultConnectChecker());
		if (camera.PrepareAudio() && camera.PrepareVideo())
		{
			camera.StartStream("rtmp://localhost:1935/live/demo");
		}
	}
}

internal class BallonDismissListener(Action action) : Java.Lang.Object, IOnBalloonDismissListener
{
	public void OnBalloonDismiss()
	{
		action();
	}
}