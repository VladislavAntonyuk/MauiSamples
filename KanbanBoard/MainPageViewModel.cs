namespace KanbanBoard;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbanBoard.Db;
using KanbanBoard.Models;

public partial class MainPageViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<ColumnInfo> columns = new();
	[ObservableProperty]
	private int position;

	private Card? dragCard;
	private readonly ICardsRepository cardsRepository;
	private readonly IColumnsRepository columnsRepository;

	public bool IsDragging => dragCard != null;

	public MainPageViewModel(ICardsRepository cardsRepository, IColumnsRepository columnsRepository)
	{
		this.cardsRepository = cardsRepository;
		this.columnsRepository = columnsRepository;
		RefreshCommand.Execute(null);
	}

	[RelayCommand]
	async Task Drop(ColumnInfo? columnInfo)
	{
		if (dragCard is null || columnInfo is null || columnInfo.Column.Cards.Count >= columnInfo.Column.Wip) return;

		var cardToUpdate = await cardsRepository.GetItem(dragCard.Id);
		if (cardToUpdate is not null)
		{
			cardToUpdate.ColumnId = columnInfo.Column.Id;
			await cardsRepository.UpdateItem(cardToUpdate);
			await UpdateCardsOrder(columnInfo.Column);
		}

		await Refresh();
		Position = columnInfo.Index;
	}

	[RelayCommand]
	void ItemDragOver(Card card)
	{
		Debug.WriteLine($"ItemDraggedOver : {card.Name}");
	}

	[RelayCommand]
	void DragOver(string direction)
	{
		switch ((SwipeDirection)Convert.ToInt32(direction))
		{
			case SwipeDirection.Left:
				if (Position < Columns.Count - 1)
				{
					Position++;
				}

				break;
			case SwipeDirection.Right:
				if (Position > 0)
				{
					Position--;
				}

				break;
		}
	}

	[RelayCommand]
	void DragStarting(Card card)
	{
		dragCard = card;
		OnPropertyChanged(nameof(IsDragging));
	}

	[RelayCommand]
	void DropCompleted()
	{
		dragCard = null;
		OnPropertyChanged(nameof(IsDragging));
	}

	[RelayCommand]
	async Task AddColumn()
	{
		var columnName = await UserPromptAsync("New column", "Enter column name", Keyboard.Default);
		if (string.IsNullOrWhiteSpace(columnName)) return;

		int wip;
		do
		{
			var wipString = await UserPromptAsync("New column", "Enter column WIP", Keyboard.Numeric);
			if (string.IsNullOrWhiteSpace(wipString)) return;

			int.TryParse(wipString, out wip);
		} while (wip < 0);

		var column = new Column { Name = columnName, Wip = wip, Order = Columns.Count + 1 };
		await columnsRepository.SaveItem(column);
		await Refresh();
		await ToastAsync("Column is added");
	}

	[RelayCommand]
	async Task AddCard(int columnId)
	{
		var column = await columnsRepository.GetItem(columnId);
		if (column is null) return;
		var columnInfo = new ColumnInfo(0, column);
		if (columnInfo.IsWipReached)
		{
			await WipReachedToastAsync("WIP is reached");
			return;
		}

		var cardName = await UserPromptAsync("New card", "Enter card name", Keyboard.Default);
		if (string.IsNullOrWhiteSpace(cardName)) return;

		var cardDescription = await UserPromptAsync("New card", "Enter card description", Keyboard.Default);
		await cardsRepository.SaveItem(new Card
		{
			Name = cardName,
			Description = cardDescription,
			ColumnId = columnId,
			Order = column.Cards.Count + 1
		});

		await Refresh();
		await ToastAsync("Card is added");
	}

	[RelayCommand]
	async Task DeleteCard(Card? card)
	{
		if (card is null) return;
		var result = await AlertAsync("Delete card", $"Do you want to delete card \"{card.Name}\"?");
		if (!result) return;

		await SnackbarAsync("The card is removed", "Cancel", async () =>
		{
			await ToastAsync("Task is cancelled");
			await cardsRepository.SaveItem(card);
			await Refresh();
		});
		await cardsRepository.DeleteItem(card.Id);
		await Refresh();
	}

	[RelayCommand]
	async Task DeleteColumn(ColumnInfo? columnInfo)
	{
		if (columnInfo is null) return;
		var result = await AlertAsync("Delete column",
			$"Do you want to delete column \"{columnInfo.Column.Name}\" and all its cards?");
		if (!result) return;

		await SnackbarAsync("The column is removed", "Cancel", async () =>
		{
			await columnsRepository.SaveItem(columnInfo.Column);
			await Refresh();
		});

		await columnsRepository.DeleteItem(columnInfo.Column.Id);
		await Refresh();
	}

	[RelayCommand]
	private async Task Refresh()
	{
		var items = await columnsRepository.GetItems();
		Columns = items
			.OrderBy(c => c.Order)
			.ToList()
			.Select(OrderCards)
			.ToObservableCollection();
		Position = 0;
	}

	private static ColumnInfo OrderCards(Column c, int columnNumber)
	{
		c.Cards = c.Cards.OrderBy(card => card.Order).ToObservableCollection();
		return new ColumnInfo(columnNumber, c);
	}

	private static Task<bool> AlertAsync(string title, string message)
	{
		return Application.Current?.MainPage is null ?
			Task.FromResult(false) :
			Application.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
	}

	private static Task<string> UserPromptAsync(string title, string message, Keyboard keyboard)
	{
		return Application.Current?.MainPage is null ?
			Task.FromResult(string.Empty) :
			Application.Current.MainPage.DisplayPromptAsync(title, message, keyboard: keyboard);
	}

	private static Task SnackbarAsync(string title, string buttonText, Action action)
	{
		return Snackbar.Make(title, action, buttonText, TimeSpan.FromSeconds(3)).Show();
	}

	private static Task ToastAsync(string title)
	{
		return Toast.Make(title, ToastDuration.Long).Show();
	}

	private static Task WipReachedToastAsync(string title)
	{
		return Toast.Make(title, ToastDuration.Long, 26d).Show();
	}

	private async Task UpdateCardsOrder(Column column)
	{
		var cards = column.Cards.OrderBy(x => x.Order).ToList();
		for (int i = 0; i < cards.Count; i++)
		{
			column.Cards[i].Order = i;
			await cardsRepository.UpdateItem(column.Cards[i]);
		}
	}
}