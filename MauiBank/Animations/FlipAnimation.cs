namespace MauiBank.Animations;

public class FlipAnimation : CommunityToolkit.Maui.Animations.BaseAnimation
{
	public enum FlipDirection
	{
		Left,
		Right
	}

	public static readonly BindableProperty DirectionProperty =
		BindableProperty.Create(nameof(Direction), typeof(FlipDirection), typeof(FlipAnimation), FlipDirection.Right,
								BindingMode.TwoWay);

	public FlipDirection Direction
	{
		get { return (FlipDirection)GetValue(DirectionProperty); }
		set { SetValue(DirectionProperty, value); }
	}

	Animation Flip(VisualElement view)
	{
		var animation = new Animation();

		animation.WithConcurrent((f) => view.Opacity = f, 0.5, 1);
		animation.WithConcurrent((f) => view.RotationY = f, (Direction == FlipDirection.Left) ? 90 : -90, 0, Microsoft.Maui.Easing.Linear);

		return animation;
	}

	public override Task Animate(VisualElement view)
	{
		view.Animate("Flip", Flip(view), 16, 500);
		return Task.CompletedTask;
	}
}