namespace MauiDraggableView;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

public class DraggableViewHandler : ViewHandler<DraggableView, FrameworkElement>
{
	private bool isPointerPressed;
	private Point previousPoint;

	public DraggableViewHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
	{
	}

	public DraggableViewHandler() : this(ViewMapper, ViewCommandMapper)
	{
	}

	protected override void ConnectHandler(FrameworkElement platformView)
	{
		base.ConnectHandler(platformView);
		platformView.PointerPressed += PlatformViewPointerPressed;
		platformView.PointerMoved += PlatformViewOnPointerMoved;
		platformView.PointerReleased += PlatformViewPointerReleased;
	}

	private void PlatformViewPointerReleased(object sender, PointerRoutedEventArgs e)
	{
		isPointerPressed = false;
		PlatformView.ReleasePointerCapture(e.Pointer);
		e.Handled = true;
	}

	private void PlatformViewPointerPressed(object sender, PointerRoutedEventArgs e)
	{
		isPointerPressed = true;
		previousPoint = e.GetCurrentPoint(PlatformView).Position;
		PlatformView.CapturePointer(e.Pointer);
		e.Handled = true;
	}

	private void PlatformViewOnPointerMoved(object sender, PointerRoutedEventArgs e)
	{
		if (isPointerPressed)
		{
			var currentPoint = e.GetCurrentPoint(PlatformView).Position;
			var offsetX = currentPoint.X - previousPoint.X;
			var offsetY = currentPoint.Y - previousPoint.Y;

			var transform = PlatformView.RenderTransform as CompositeTransform;
			if (transform == null)
			{
				transform = new CompositeTransform();
				PlatformView.RenderTransform = transform;
			}

			transform.TranslateX += offsetX;
			transform.TranslateY += offsetY;

			previousPoint = currentPoint;
			e.Handled = true;
		}
	}

	protected override void DisconnectHandler(FrameworkElement platformView)
	{
		platformView.PointerPressed -= PlatformViewPointerPressed;
		platformView.PointerMoved -= PlatformViewOnPointerMoved;
		platformView.PointerReleased -= PlatformViewPointerReleased;
		base.DisconnectHandler(platformView);
	}

	protected override FrameworkElement CreatePlatformView()
	{
		return VirtualView.Content.ToPlatform(MauiContext ?? throw new NullReferenceException());
	}
}