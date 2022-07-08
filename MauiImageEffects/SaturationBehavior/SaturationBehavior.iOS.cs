using UIKit;

namespace MauiImageEffects.SaturationBehavior;

using CoreGraphics;
using CoreImage;

public partial class SaturationBehavior : PlatformBehavior<Image, UIImageView>
{
	private CGImage? originalImage;
	UIImageView? imageView;

	protected override void OnAttachedTo(Image bindable, UIImageView platformView)
	{
		imageView = platformView;

		originalImage = platformView.Image?.CGImage;
		SetRendererEffect(imageView, Saturation);
	}

	protected override void OnDetachedFrom(Image bindable, UIImageView platformView)
	{
		SetImage(platformView, originalImage);
	}

	static void SetImage(UIImageView imageView, CGImage? image)
	{
		if (image is null)
		{
			return;
		}

		imageView.Image = new UIImage(image);
	}

	static void SetRendererEffect(UIImageView imageView, float saturation)
	{
		if (imageView.Image?.CGImage is null)
		{
			return;
		}

		var myContext = CIContext.Create();
		var inputImage = CIImage.FromCGImage(imageView.Image.CGImage);
		var filter = new CISaturationBlendMode()
		{
			InputImage = inputImage
		};
		var resultImage = myContext.CreateCGImage(filter.OutputImage!, inputImage.Extent);
		SetImage(imageView, resultImage);
	}
}