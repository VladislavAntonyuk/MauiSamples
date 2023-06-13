namespace MauiMaps;

public class GeolocatorImplementation : IGeolocator
{
	public async Task StartListening(IProgress<Location> positionChangedProgress, CancellationToken cancellationToken)
	{
		var taskCompletionSource = new TaskCompletionSource();
		cancellationToken.Register(() =>
		{
			PositionChanged();
			taskCompletionSource.TrySetResult();
		});

		void PositionChanged()
		{
		}

		await taskCompletionSource.Task;
	}
}

public class CustomMapHandler : Microsoft.Maui.Maps.Handlers.MapHandler
{
}