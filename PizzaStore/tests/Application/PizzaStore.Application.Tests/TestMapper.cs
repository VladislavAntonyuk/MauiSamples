namespace PizzaStore.Application.Tests;

using AutoMapper;
using PizzaStore.Application.Configuration;

internal static class TestMapper
{
	static TestMapper()
	{
		var configuration = new MapperConfiguration(cfg => cfg.AddProfile(typeof(AutoMapperProfile)));

		Instance = configuration.CreateMapper();
	}

	public static IMapper Instance { get; }
}