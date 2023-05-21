namespace MauiDraggableView;

using Android.Views;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

public class DraggableViewHandler : ViewHandler<DraggableView, View>
{
	private float deltaX;
	private float deltaY;

	public DraggableViewHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
	{
	}

	public DraggableViewHandler() : this(ViewMapper, ViewCommandMapper)
	{
	}

	protected override void ConnectHandler(View platformView)
	{
		base.ConnectHandler(platformView);
		platformView.Touch += HandleTouch;
	}

	protected override void DisconnectHandler(View platformView)
	{
		platformView.Touch -= HandleTouch;
		base.DisconnectHandler(platformView);
	}

	protected override View CreatePlatformView()
	{
		return VirtualView.Content.ToPlatform(MauiContext ?? throw new NullReferenceException());
	}


	private void HandleTouch(object? sender, View.TouchEventArgs args)
	{
		var e = args.Event;
		if (e is null)
		{
			return;
		}

		float x = e.RawX;
		float y = e.RawY;
		switch (e.Action)
		{
			case MotionEventActions.Down:
				deltaX = x - PlatformView.GetX();
				deltaY = y - PlatformView.GetY();
				break;
			case MotionEventActions.Move:
				var newX = x - deltaX;
				var newY = y - deltaY;
				PlatformView.SetX(newX);
				PlatformView.SetY(newY);

				break;
		}
	}
}