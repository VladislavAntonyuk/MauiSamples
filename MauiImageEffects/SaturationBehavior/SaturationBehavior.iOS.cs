using UIKit;

namespace MauiImageEffects.SaturationBehavior;

using CoreImage;

public partial class SaturationBehavior : PlatformBehavior<Image, UIImageView>
{
	private UIImage? originalImage;
	UIImageView? imageView;

	protected override void OnAttachedTo(Image bindable, UIImageView platformView)
	{
		imageView = platformView;

		originalImage = platformView.Image;
		SetRendererEffect(imageView, Saturation);
	}

	protected override void OnDetachedFrom(Image bindable, UIImageView platformView)
	{
		SetImage(platformView, originalImage);
	}

	static void SetImage(UIImageView imageView, UIImage? image)
	{
		if (image is null)
		{
			return;
		}

		imageView.Image = image;
	}

	void SetRendererEffect(UIImageView imageView, float saturation)
	{
		if (originalImage is null)
		{
			return;
		}

		var myContext = CIContext.Create();
		var inputImage = new CIImage(originalImage);
		var filter = new CIColorControls()
		{
			InputImage = inputImage,
			Saturation = saturation
		};
		if (filter.OutputImage is null)
		{
			return;
		}

		var resultImage = myContext.CreateCGImage(filter.OutputImage, inputImage.Extent);
		if (resultImage is null)
		{
			return;
		}

		SetImage(imageView, new UIImage(resultImage));
	}
}