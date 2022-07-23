using UIKit;

namespace MauiImageEffects.ChainBehavior;

using CoreImage;

public partial class ChainBehavior : PlatformBehavior<Image, UIImageView>
{
	private UIImage? originalImage;
	UIImageView? imageView;

	protected override void OnAttachedTo(Image bindable, UIImageView platformView)
	{
		imageView = platformView;

		originalImage = platformView.Image;
		SetRendererEffect(imageView, Effects);
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

	void SetRendererEffect(UIImageView imageView, string? effects)
	{
		if (originalImage is null)
		{
			return;
		}

		var inputImage = new CIImage(originalImage);
		var ciImage = GetEffect(effects, inputImage);
		if (ciImage is null)
		{
			return;
		}

		var myContext = CIContext.Create();
		var resultImage = myContext.CreateCGImage(ciImage, inputImage.Extent);
		if (resultImage is null)
		{
			return;
		}

		SetImage(imageView, new UIImage(resultImage));
	}


	static CIImage? GetEffect(string? effects, CIImage inputImage)
	{
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
		CIFilter? filter = effectName switch
		{
			"blur" => new CIGaussianBlur
			{
				InputImage = inputImage,
				Radius = 5
			},
			"saturation" => new CIColorControls()
			{
				InputImage = inputImage,
				Saturation = 0.05f
			},
			_ => null
		};


		return filter?.OutputImage;
	}

	static CIImage? CreateChainEffect(string[] effects, CIImage inputImage)
	{
		var outputImage = CreateEffectByName(effects[0], inputImage);
		if (effects.Length == 1 || outputImage is null)
		{
			return outputImage;
		}

		var innerEffectNames = effects[1..];

		return CreateChainEffect(innerEffectNames, outputImage);
	}
}