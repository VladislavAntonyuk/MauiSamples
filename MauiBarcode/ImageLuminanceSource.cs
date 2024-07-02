namespace MauiBarcode;

using Microsoft.Maui.Graphics.Platform;
using ZXing;

public class ImageLuminanceSource : RGBLuminanceSource
{
	public ImageLuminanceSource(PlatformImage image)
		: this((int)image.Width, (int)image.Height)
	{

#if ANDROID
		var pixels = new int[(int)image.Width * (int)image.Height];
		image.PlatformRepresentation.GetPixels(pixels, 0, (int)image.Width, 0, 0, (int)image.Width, (int)image.Height);
		var rgbRawBytes = new byte[pixels.Length * 4];
		Buffer.BlockCopy(pixels, 0, rgbRawBytes, 0, rgbRawBytes.Length);
#elif IOS || MACCATALYST
		var pixels = new int[(int)image.Width * (int)image.Height];
		var rgbRawBytes = new byte[pixels.Length * 4];
#elif WINDOWS
		var rgbRawBytes = image.PlatformRepresentation.GetPixelBytes();
#else
		var pixels = new int[(int)image.Width * (int)image.Height];
		var rgbRawBytes = new byte[pixels.Length * 4];
#endif

		CalculateLuminance(rgbRawBytes, BitmapFormat.RGB32);
	}

	protected ImageLuminanceSource(int width, int height)
		: base(width, height)
	{
	}

	protected override LuminanceSource CreateLuminanceSource(byte[] newLuminances, int width, int height)
	{
		return new ImageLuminanceSource(width, height) { luminances = newLuminances };
	}
}