namespace MauiAnimation.Controls;

using Animations;

public partial class Sky : ContentView
{
	public Sky()
	{
		InitializeComponent();
		var animation = new CloudAnimation();
		animation.Length = 5000;
		animation.Animate(Cloud1);
		animation.Length = 7000;
		animation.Animate(Cloud2);
		animation.Length = 9000;
		animation.Animate(Cloud3);
		animation.Length = 3000;
		var planeAnimation = new PlaneAnimation(Plane, GloryToUkraine);
		planeAnimation.Animate(Plane);
	}
}