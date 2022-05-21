namespace PizzaStore.Mobile.ViewModels;

using System.Collections.ObjectModel;
using Application.Interfaces.CQRS;
using Application.UseCases.Pizza;
using Application.UseCases.Pizza.Commands.Delete;
using Application.UseCases.Pizza.Queries.GetPizza;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class BasketViewModel : ObservableObject
{
	private readonly ICommandDispatcher commandDispatcher;
	private readonly IQueryDispatcher queryDispatcher;

	[ObservableProperty]
	private ObservableCollection<PizzaDto> items = new();

	public BasketViewModel(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
	{
		this.queryDispatcher = queryDispatcher;
		this.commandDispatcher = commandDispatcher;
		GetItemsCommand.Execute(null);
	}

	[ICommand]
	private async Task GetItems(CancellationToken cancellationToken)
	{
		var result = await queryDispatcher.SendAsync<GetPizzaByFilterResponse, GetPizzaQuery>(new GetPizzaQuery
		{
			Limit = 25
		}, cancellationToken);
		if (result.IsSuccessful)
		{
			items.Clear();
			foreach (var item in result.Value.Items)
			{
				items.Add(item);
			}
		}
		else
		{
			var errors = string.Join(Environment.NewLine, result.Errors);
			await Toast.Make(errors, ToastDuration.Long).Show(cancellationToken);
		}
	}

	[ICommand]
	private async Task DeleteItem(int itemId, CancellationToken cancellationToken)
	{
		var result = await commandDispatcher.SendAsync<bool, DeletePizzaCommand>(new DeletePizzaCommand(itemId), cancellationToken);
		if (result.IsSuccessful)
		{
			await GetItems(cancellationToken);
		}
		else
		{
			var errors = string.Join(Environment.NewLine, result.Errors);
			await Toast.Make(errors, ToastDuration.Long).Show(cancellationToken);
		}
	}
}