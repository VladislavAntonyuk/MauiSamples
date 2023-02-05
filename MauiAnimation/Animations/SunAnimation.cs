namespace MauiAnimation.Animations;

public class SunAnimation : CommunityToolkit.Maui.Animations.BaseAnimation
{
	Animation Sun(VisualElement view)
	{
		var animation = new Animation();

		animation.WithConcurrent((f) => view.Rotation = f, 0, 360, Microsoft.Maui.Easing.Linear);
		animation.WithConcurrent((f) => view.Scale = f, 1, 1.4, Microsoft.Maui.Easing.Linear);

		return animation;
	}

	public override Task Animate(VisualElement view)
	{
		view.Animate("Sun", Sun(view), 16, 3500, repeat: () => true);
		return Task.CompletedTask;
	}
}