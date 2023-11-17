namespace MauiAnimation.Animations;

using CommunityToolkit.Maui.Animations;

public class PlaneAnimation : BaseAnimation
{
	private readonly VisualElement plane;
	private readonly VisualElement gloryToUkraine;

	public PlaneAnimation(VisualElement plane, VisualElement gloryToUkraine)
	{
		this.plane = plane;
		this.gloryToUkraine = gloryToUkraine;
	}

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