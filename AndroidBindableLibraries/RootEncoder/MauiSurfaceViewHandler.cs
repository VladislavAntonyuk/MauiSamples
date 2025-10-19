namespace RtpLibrary;

using Android.Views;
using Android.Widget;
using Microsoft.Maui.Handlers;

public class MauiSurfaceViewHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null)
	: ViewHandler<IMauiSurfaceView, SurfaceView>(mapper, commandMapper)
{

	public static IPropertyMapper<IMauiSurfaceView, MauiSurfaceViewHandler> PropertyMapper = new PropertyMapper<IMauiSurfaceView, MauiSurfaceViewHandler>(ViewMapper)
	{
	};

	public static CommandMapper<IMauiSurfaceView, MauiSurfaceViewHandler> CommandMapper = new(ViewCommandMapper);

	public MauiSurfaceViewHandler() : this(PropertyMapper, CommandMapper)
	{
	}

	protected override VideoView CreatePlatformView()
	{
		return new VideoView(Context)
		{
			Focusable = true,
			FocusableInTouchMode = true
		};
	}
}