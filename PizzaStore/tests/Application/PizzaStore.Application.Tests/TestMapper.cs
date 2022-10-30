namespace PizzaStore.Application.Tests;

using PizzaStore.Application.Configuration;
using AutoMapper;

internal static class TestMapper
{
	static TestMapper()
	{
		var configuration = new MapperConfiguration(cfg => cfg.AddProfile(typeof(AutoMapperProfile)));

		Instance = configuration.CreateMapper();
	}

	public static IMapper Instance { get; }
}