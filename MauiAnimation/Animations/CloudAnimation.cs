namespace MauiAnimation.Animations;

using MauiAnimation.Controls;

public class CloudAnimation : CommunityToolkit.Maui.Animations.BaseAnimation<Cloud>
{
	Animation Cloud(Cloud view)
	{
		var animation = new Animation();

		animation.WithConcurrent((f) => view.TranslationX = f, view.TranslationX - 500, view.TranslationX + 1200, Microsoft.Maui.Easing.Linear);
		animation.WithConcurrent((f) => view.TranslationY = f, view.TranslationY, view.TranslationY + 300, Microsoft.Maui.Easing.Linear);
		animation.WithConcurrent((f) => view.Scale = f, 1, 1.5, Microsoft.Maui.Easing.Linear);

		return animation;
	}

	public override Task Animate(Cloud view)
	{
		view.Animate("Cloud", Cloud(view), 16, Length, repeat: () => true);
		return Task.CompletedTask;
	}
}