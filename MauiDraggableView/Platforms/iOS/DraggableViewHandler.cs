namespace MauiDraggableView;

using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

public class DraggableViewHandler : ViewHandler<DraggableView, UIView>
{
	private double translatedXPos;
	private double translatedYPos;

	private UIPanGestureRecognizer panGesture;

	public DraggableViewHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
	{
		panGesture = new UIPanGestureRecognizer(DetectPan);
	}

	public DraggableViewHandler() : this(ViewMapper, ViewCommandMapper)
	{
	}

	protected override void ConnectHandler(UIView platformView)
	{
		base.ConnectHandler(platformView);
		platformView.AddGestureRecognizer(panGesture);
	}

	protected override void DisconnectHandler(UIView platformView)
	{
		platformView.RemoveGestureRecognizer(panGesture);
		base.DisconnectHandler(platformView);
	}

	protected override UIView CreatePlatformView()
	{
		return VirtualView.Content.ToPlatform(MauiContext ?? throw new NullReferenceException());
	}

	private void DetectPan()
	{
		if (panGesture.State == UIGestureRecognizerState.Began)
		{
			panGesture.Reset();
			translatedXPos = VirtualView.TranslationX;
			translatedYPos = VirtualView.TranslationY;
		}

		var translation = panGesture.TranslationInView(PlatformView.Superview);
		VirtualView.TranslationX = translatedXPos + translation.X;
		VirtualView.TranslationY = translatedYPos + translation.Y;
	}
}