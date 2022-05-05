namespace PizzaStore.Application.Configuration;

using AutoMapper;
using Domain.Entities;
using UseCases;
using UseCases.Pizza;
using UseCases.Pizza.Commands.Create;
using UseCases.Pizza.Commands.Delete;
using UseCases.Pizza.Commands.Update;
using UseCases.Pizza.Queries.GetPizza;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		MapPizza();
	}

	private void MapPizza()
	{
		CreateMap<Pizza, PizzaDto>().ReverseMap();
		CreateMap<PaginatedList<Pizza>, GetPizzaByFilterResponse>();
		CreateMap<CreatePizzaCommand, Pizza>();
		CreateMap<UpdatePizzaCommand, Pizza>();
		CreateMap<DeletePizzaCommand, Pizza>();
	}
}