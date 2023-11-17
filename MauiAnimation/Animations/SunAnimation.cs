namespace MauiAnimation.Animations;

using CommunityToolkit.Maui.Animations;

public class SunAnimation : BaseAnimation
{
	Animation Sun(VisualElement view)
	{
		var animation = new Animation();

		animation.WithConcurrent(f => view.Rotation = f, 0, 360, Easing.Linear);
		animation.WithConcurrent(f => view.Scale = f, 1, 1.4, Easing.Linear);

		return animation;
	}

	public override Task Animate(VisualElement view, CancellationToken cancellationToken = default)
	{
		view.Animate("Sun", Sun(view), 16, 3500, repeat: () => true);
		return Task.CompletedTask;
	}
}