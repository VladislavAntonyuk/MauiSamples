namespace PizzaStore.Mobile.ViewModels;

using System.Collections.ObjectModel;
using Application.Interfaces.CQRS;
using Application.UseCases.Pizza;
using Application.UseCases.Pizza.Commands.Update;
using Application.UseCases.Pizza.Queries.GetPizza;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
		GetItemsCommand.Execute(null);
	}

	[RelayCommand]
	async Task GetItems(CancellationToken cancellationToken)
	{
		var result = await queryDispatcher.SendAsync<GetPizzaByFilterResponse, GetPizzaQuery>(new GetPizzaQuery
		{
			Limit = 25
		}, cancellationToken);
		if (result.IsSuccessful)
		{
			Items.Clear();
			foreach (var item in result.Value.Items)
			{
				Items.Add(item);
			}
		}
		else
		{
			var errors = string.Join(Environment.NewLine, result.Errors);
			await Toast.Make(errors, ToastDuration.Long).Show(cancellationToken);
		}
	}

	[RelayCommand]
	async Task Buy(int itemId, CancellationToken cancellationToken)
	{
		var result = await commandDispatcher.SendAsync<PizzaDto, UpdatePizzaCommand>(new UpdatePizzaCommand(itemId)
		{
			Name = DateTime.Now.ToString("O")
		}, cancellationToken);
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