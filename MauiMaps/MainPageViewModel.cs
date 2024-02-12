namespace MauiMaps;

using System.Collections.ObjectModel;
using AutoFixture;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

public partial class MainPageViewModel(IFixture fixture) : ObservableObject
{
	private LocationPin? currentLocationPin;

	[RelayCommand]
	private void Add()
	{
		var newLocation = fixture.Build<LocationPin>()
								 .With(x => x.ImageSource, ImageSource.FromUri(new Uri($"https://picsum.photos/{Random.Shared.Next(40, 60)}")))
								 .With(x => x.Location, new Location(Random.Shared.Next(44, 51), Random.Shared.Next(23, 40)))
								 .Create();
		LocationPins.Add(newLocation);
	}

	[RelayCommand]
	private void Remove()
	{
		if (LocationPins.Count > 0)
		{
			LocationPins.RemoveAt(LocationPins.Count - 1);
		}
	}

	[RelayCommand]
	private void RemoveAll()
	{
		LocationPins.Clear();
	}

	[RelayCommand(IncludeCancelCommand = true, AllowConcurrentExecutions = false)]
	private async Task RealTimeLocationTracker(CancellationToken cancellationToken)
	{
		var progress = new Progress<Location>(location =>
		{
			if (currentLocationPin is null)
			{
				currentLocationPin = new LocationPin
				{
					ImageSource = ImageSource.FromUri(new Uri($"https://picsum.photos/{Random.Shared.Next(40, 60)}")),
					Location = location,
					Description = "I am here!"
				};
			}
			else
			{
				LocationPins.Remove(currentLocationPin);
				currentLocationPin.Location = location;
			}

			LocationPins.Add(currentLocationPin);
		});
		await Geolocator.Default.StartListening(progress, cancellationToken);
	}

	public ObservableCollection<LocationPin> LocationPins { get; } = new();
}