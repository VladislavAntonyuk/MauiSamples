using UIKit;

namespace MauiImageEffects.ChainBehavior;

using CoreGraphics;
using CoreImage;

public partial class ChainBehavior : PlatformBehavior<Image, UIImageView>
{
	private CGImage? originalImage;
	UIImageView? imageView;

	protected override void OnAttachedTo(Image bindable, UIImageView platformView)
	{
		imageView = platformView;

		originalImage = platformView.Image?.CGImage;
		SetRendererEffect(imageView, null);
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

	static void SetRendererEffect(UIImageView imageView, string? effects)
	{
		if (imageView.Image?.CGImage is null)
		{
			return;
		}

		var inputImage = CIImage.FromCGImage(imageView.Image.CGImage);
		var ciImage = GetEffect(effects, inputImage);
		var myContext = CIContext.Create();
		var resultImage = myContext.CreateCGImage(ciImage!, inputImage.Extent);
		SetImage(imageView, resultImage);
	}


	static CIImage? GetEffect(string? effects, CIImage inputImage)
	{
		if (!OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			return null;
		}

		var effectNames = effects?.Split(',', StringSplitOptions.RemoveEmptyEntries);
		return effectNames?.Length switch
		{
			null => null,
			1 => CreateEffectByName(effectNames[0], inputImage),
			_ => CreateChainEffect(effectNames, inputImage)
		};
	}

	static CIImage? CreateEffectByName(string effectName, CIImage inputImage)
	{
		if (!OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			return null;
		}
		
		CIFilter? filter = effectName switch
		{
			"blur" => new CIGaussianBlur
			{
				InputImage = inputImage,
				Radius = 5
			},
			"saturation" => new CISaturationBlendMode()
			{
				InputImage = inputImage
			},
			_ => null
		};


		return filter?.OutputImage;
	}

	static CIImage? CreateChainEffect(string[] effects, CIImage inputImage)
	{
		if (!OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			return null;
		}

		var outputImage = CreateEffectByName(effects[0], inputImage);
		if (effects.Length == 1)
		{
			return outputImage;
		}

		var innerEffectNames = effects[1..];

		return CreateChainEffect(innerEffectNames, outputImage!);
	}
}