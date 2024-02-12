namespace MauiAnimation.Animations;

using CommunityToolkit.Maui.Animations;

public class PlaneAnimation(VisualElement plane, VisualElement gloryToUkraine) : BaseAnimation
{
	public override Task Animate(VisualElement view, CancellationToken cancellationToken = default)
	{
		plane.Animate("Plane", f =>
		{
			plane.TranslationX = f;
			if (f > 500)
			{
				gloryToUkraine.Opacity = Math.Sin((f - 1000) / 2000);
			}
			else
			{
				gloryToUkraine.Opacity = 0;
			}
		}, plane.TranslationX - 200, 3000, 16, 7000, repeat: () => true);
		return Task.CompletedTask;
	}
}