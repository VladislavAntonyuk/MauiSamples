namespace PizzaStore.Infrastructure.WebApp.Data.Tests.Configuration.Mappings;

using AutoMapper;
using Data.Configuration;

internal static class TestMapper
{
	static TestMapper()
	{
		var configuration = new MapperConfiguration(cfg => cfg.AddProfile(typeof(AutoMapperProfile)));

		Instance = configuration.CreateMapper();
	}

	public static IMapper Instance { get; }
}