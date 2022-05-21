namespace PizzaStore.Infrastructure.Mobile.Data.Configuration;

using AutoMapper;
using Repositories.Models;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		SetupBanner();
	}

	private void SetupBanner()
	{
		CreateMap<Pizza, Domain.Entities.Pizza>().ReverseMap();
	}
}