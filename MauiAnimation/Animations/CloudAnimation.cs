namespace MauiAnimation.Animations;

using CommunityToolkit.Maui.Animations;
using Controls;

public class CloudAnimation : BaseAnimation<Cloud>
{
	Animation Cloud(Cloud view)
	{
		var animation = new Animation();

		animation.WithConcurrent(f => view.TranslationX = f, view.TranslationX - 500, view.TranslationX + 1200, Easing.Linear);
		animation.WithConcurrent(f => view.TranslationY = f, view.TranslationY, view.TranslationY + 300, Easing.Linear);
		animation.WithConcurrent(f => view.Scale = f, 1, 1.5, Easing.Linear);

		return animation;
	}

	public override Task Animate(Cloud view, CancellationToken cancellationToken = default)
	{
		view.Animate("Cloud", Cloud(view), 16, Length, repeat: () => true);
		return Task.CompletedTask;
	}
}