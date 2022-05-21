namespace PizzaStore.WebApp.Pages;

using PizzaStore.Application.Interfaces.CQRS;
using PizzaStore.Application.UseCases.Pizza;
using PizzaStore.Application.UseCases.Pizza.Commands.Create;
using PizzaStore.Application.UseCases.Pizza.Commands.Delete;
using PizzaStore.Application.UseCases.Pizza.Commands.Update;
using PizzaStore.Application.UseCases.Pizza.Queries.GetPizza;
using Microsoft.AspNetCore.Components;
using MudBlazor;

public partial class FetchData : PizzaStoreBaseComponent
{
	[Inject]
	public IQueryDispatcher QueryDispatcher { get; set; } = null!;

	[Inject]
	public ICommandDispatcher CommandDispatcher { get; set; } = null!;
	
	[Inject]
	public ISnackbar Snackbar { get; set; } = null!;

	private MudTable<PizzaDto> table = null!;
	private MudTextField<string>? searchString;
	private readonly System.Windows.Input.ICommand updateCommand;
	private readonly System.Windows.Input.ICommand deleteCommand;

	public FetchData()
	{
		updateCommand = new ModelCommand<int>(async id => await Update(id));
		deleteCommand = new ModelCommand<int>(async id => await Delete(id));
	}

	private async Task<TableData<PizzaDto>> LoadPizzas(TableState state)
	{
		var result = await QueryDispatcher.SendAsync<GetPizzaByFilterResponse, GetPizzaQuery>(new GetPizzaQuery
		{
			Limit = state.PageSize,
			Name = searchString?.Value,
			Offset = state.Page
		});
		if (result.IsSuccessful)
		{
			return new TableData<PizzaDto>
			{
				TotalItems = result.Value.TotalCount,
				Items = result.Value.Items
			};
		}

		return new TableData<PizzaDto>();
	}

	async Task CreatePizza()
	{
		var result = await CommandDispatcher.SendAsync<PizzaDto, CreatePizzaCommand>(new CreatePizzaCommand
		{
			Name = DateTime.Now.ToString("O")
		});
		if (result.IsSuccessful)
		{
			Snackbar.Add("Created", Severity.Success);
			await table.ReloadServerData();
		}
		else
		{
			Snackbar.Add(result.Errors.FirstOrDefault("Error has occurred"), Severity.Error);
		}
	}
	
	private Task OnSearch(string text)
	{
		return table.ReloadServerData();
	}
	
	private async Task Delete(int id)
	{
		var result = await CommandDispatcher.SendAsync<bool, DeletePizzaCommand>(new DeletePizzaCommand(id));
		if (result.IsSuccessful)
		{
			Snackbar.Add("Deleted", Severity.Success);
			await table.ReloadServerData();
		}
		else
		{
			Snackbar.Add(result.Errors.FirstOrDefault("Error has occurred"), Severity.Error);
		}
	}
	
	private async Task Update(int id)
	{
		var result = await CommandDispatcher.SendAsync<PizzaDto, UpdatePizzaCommand>(new UpdatePizzaCommand(id)
		{
			Name = DateTime.Now.ToString("O")
		});
		if (result.IsSuccessful)
		{
			Snackbar.Add("Updated", Severity.Success);
			await table.ReloadServerData();
		}
		else
		{
			Snackbar.Add(result.Errors.FirstOrDefault("Error has occurred"), Severity.Error);
		}
	}
}
