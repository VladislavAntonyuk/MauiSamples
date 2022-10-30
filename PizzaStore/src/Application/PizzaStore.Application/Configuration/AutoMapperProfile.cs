namespace PizzaStore.Application.Configuration;

using System.Collections.ObjectModel;
using AutoMapper;
using Domain.Entities;
using Interfaces;
using UseCases;
using UseCases.Pizza;
using UseCases.Pizza.Commands.Create;
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
		CreateMap<PaginatedList<Pizza>, GetPizzaByFilterResponse>()
			.ForMember(x => x.Items, dest => dest.MapFrom(x => x.Items.ToList()));
		CreateMap<IEnumerable<Pizza>, GetPizzaByFilterResponse>()
			.ForMember(x => x.Items, dest => dest.MapFrom(x => x.ToList()))
			.ForMember(x => x.TotalCount, dest => dest.MapFrom(x => x.Count()));

		CreateMap<CreatePizzaCommand, Pizza>()
			.Ignore(x => x.Id)
			.Ignore(x => x.CreatedBy)
			.Ignore(x => x.CreatedOn)
			.Ignore(x => x.ModifiedBy)
			.Ignore(x => x.ModifiedOn);
		CreateMap<UpdatePizzaCommand, Pizza>()
			.Ignore(x => x.CreatedBy)
			.Ignore(x => x.CreatedOn)
			.Ignore(x => x.ModifiedBy)
			.Ignore(x => x.ModifiedOn);
	}
}