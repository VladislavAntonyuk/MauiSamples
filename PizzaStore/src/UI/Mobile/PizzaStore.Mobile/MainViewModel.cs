namespace PizzaStore.Mobile;

using System.Collections.ObjectModel;
using Application.Interfaces.CQRS;
using Application.UseCases.Pizza;
using Application.UseCases.Pizza.Commands.Create;
using Application.UseCases.Pizza.Queries.GetPizza;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MainViewModel : ObservableObject
{
	private readonly IQueryDispatcher queryDispatcher;
	private readonly ICommandDispatcher commandDispatcher;

	[ObservableProperty]
	private ObservableCollection<PizzaDto> items = new();

	public MainViewModel(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
	{
		this.queryDispatcher = queryDispatcher;
		this.commandDispatcher = commandDispatcher;
	}

	[ICommand]
	async Task GetItems(CancellationToken cancellationToken)
	{
		var result = await queryDispatcher.SendAsync<GetPizzaByFilterResponse, GetPizzaQuery>(new GetPizzaQuery
		{
			Limit = 25
		}, cancellationToken);
		if (result.IsSuccessful)
		{
			items = new ObservableCollection<PizzaDto>(result.Value.Items);
		}
	}

	[ICommand]
	async Task CreateItem(CancellationToken cancellationToken)
	{
		var result = await commandDispatcher.SendAsync<PizzaDto, CreatePizzaCommand>(new CreatePizzaCommand
		{
			Name = Path.GetRandomFileName()
		}, cancellationToken);
		if (result.IsSuccessful)
		{
			await GetItems(cancellationToken);
		}
	}
}
