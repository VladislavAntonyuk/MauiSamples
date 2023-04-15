namespace MauiMaps;

public interface IGeolocator
{
	Task StartListening(IProgress<Location> positionChangedProgress, CancellationToken cancellationToken);
}