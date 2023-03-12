namespace MauiMaps;

using System.Collections.ObjectModel;
using AutoFixture;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

public partial class MainPageViewModel : ObservableObject
{
	private readonly IFixture fixture;

	public MainPageViewModel(IFixture fixture)
	{
		this.fixture = fixture;
	}

	[RelayCommand]
	private void Add()
	{
		var newLocation = fixture.Build<LocationPin>()
								 .With(x => x.ImageSource, ImageSource.FromUri(new Uri($"https://picsum.photos/{Random.Shared.Next(40, 60)}")))
								 .With(x => x.Location, new Location(Random.Shared.Next(5, 15), Random.Shared.Next(5, 15)))
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

	public ObservableCollection<LocationPin> LocationPins { get; } = new();
}