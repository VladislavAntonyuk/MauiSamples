namespace PizzaStore.Application.Tests;

using Xunit;

public class AutoMapperTest
{
	[Fact]
	public void AutoMapper_Test_All_Mappings()
	{
		TestMapper.Instance.ConfigurationProvider.AssertConfigurationIsValid();
	}
}