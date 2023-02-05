namespace MauiBlazorPhotoGallery.Data;
public class MediaService
{
	public IEnumerable<MediaItem> GetMediaItems(int imageCount)
	{
		for (int i = 0; i < imageCount; i++)
		{
			yield return new MediaItem()
			{
				Title = $"Image {i}",
				Link = $"https://picsum.photos/{Random.Shared.Next(100, 500)}/{Random.Shared.Next(100, 500)}"
			};
		}
	}
}