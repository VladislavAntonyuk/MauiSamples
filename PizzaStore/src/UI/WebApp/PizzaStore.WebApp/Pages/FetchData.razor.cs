namespace PizzaStore.WebApp.Pages;

using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PizzaStore.Application.Interfaces.CQRS;
using PizzaStore.Application.UseCases.Pizza;
using PizzaStore.Application.UseCases.Pizza.Commands.Create;
using PizzaStore.Application.UseCases.Pizza.Commands.Delete;
using PizzaStore.Application.UseCases.Pizza.Commands.Update;
using PizzaStore.Application.UseCases.Pizza.Queries.GetPizza;

public partial class FetchData : PizzaStoreBaseComponent, IDisposable
{
	private readonly ICommand deleteCommand;
	private readonly ICommand updateCommand;
	private MudTextField<string>? searchString;

	private MudTable<PizzaDto> table = null!;

	public FetchData()
	{
		updateCommand = new ModelCommand<int>(async id => await Update(id));
		deleteCommand = new ModelCommand<int>(async id => await Delete(id));
	}

	[Inject]
	public IQueryDispatcher QueryDispatcher { get; set; } = null!;

	[Inject]
	public ICommandDispatcher CommandDispatcher { get; set; } = null!;

	[Inject]
	public ISnackbar Snackbar { get; set; } = null!;

	private async Task<TableData<PizzaDto>> LoadPizzas(TableState state)
	{
		var result = await QueryDispatcher.SendAsync<GetPizzaByFilterResponse, GetPizzaQuery>(new GetPizzaQuery
		{
			Limit = state.PageSize,
			Name = searchString?.Value,
			Offset = state.Page
		}, CancellationToken.None);
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

	private async Task CreatePizza()
	{
		var result = await CommandDispatcher.SendAsync<PizzaDto, CreatePizzaCommand>(new CreatePizzaCommand
		{
			Name = DateTime.Now.ToString("O")
		}, CancellationToken.None);
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
		var result = await CommandDispatcher.SendAsync<bool, DeletePizzaCommand>(new DeletePizzaCommand(id), CancellationToken.None);
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
		}, CancellationToken.None);
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

	public void Dispose()
	{
		Snackbar.Dispose();
	}
}