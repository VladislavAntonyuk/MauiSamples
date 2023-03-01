namespace MauiMaps.Models;

public class LocationPin
{
	public string? Description { get; set; }
	public string? Address { get; set; }
	public Location? Location { get; set; }
	public ImageSource? ImageSource { get; set; }
}