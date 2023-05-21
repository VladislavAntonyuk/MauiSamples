namespace MauiDraggableView;

using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

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
		platformView.TouchEvent += HandleTouch;
	}

	protected override void DisconnectHandler(View platformView)
	{
		platformView.TouchEvent -= HandleTouch;
		base.DisconnectHandler(platformView);
	}

	protected override View CreatePlatformView()
	{
		return VirtualView.Content.ToPlatform(MauiContext ?? throw new NullReferenceException());
	}


	private bool HandleTouch(object? sender, View.TouchEventArgs args)
	{
		var e = args.Touch;
		if (e is null)
		{
			return false;
		}

		float x = e.GetScreenPosition(0).X;
		float y = e.GetScreenPosition(0).Y;
		switch (e.GetState(0))
		{
			case PointStateType.Down:
				deltaX = x - PlatformView.PositionX;
				deltaY = y - PlatformView.PositionY;
				break;
			case PointStateType.Motion:
				var newX = x - deltaX;
				var newY = y - deltaY;
				PlatformView.PositionX = newX;
				PlatformView.PositionY = newY;

				break;
		}

		return true;
	}
}