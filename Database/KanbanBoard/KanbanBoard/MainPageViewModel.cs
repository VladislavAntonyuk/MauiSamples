namespace KanbanBoard;

using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbanBoardDb;
using Microsoft.EntityFrameworkCore;
using Models;

public partial class MainPageViewModel : ObservableObject
{
	private readonly KanbanBoardDbContext dbContext;

	[ObservableProperty]
	public partial ObservableCollection<ColumnInfo> Columns { get; set; }

	[ObservableProperty]
	public partial int Position { get; set; }

	private CardInfo? dragCard;

	public bool IsDragging => dragCard != null;

	public MainPageViewModel(KanbanBoardDbContext dbContext)
	{
		this.dbContext = dbContext;
		Columns = new();
		RefreshCommand.Execute(null);
	}

	[RelayCommand]
	async Task Drop(ColumnInfo? columnInfo)
	{
		if (dragCard is null || columnInfo is null || columnInfo.KanbanColumn.Cards.Count >= columnInfo.KanbanColumn.Wip) return;

		var cardToUpdate = await dbContext.Cards.FirstOrDefaultAsync(x => x.Id == dragCard.KanbanCard.Id);
		if (cardToUpdate is not null)
		{
			cardToUpdate.ColumnId = columnInfo.KanbanColumn.Id;
			UpdateCardsOrder(columnInfo.KanbanColumn);
			await dbContext.SaveChangesAsync();
		}

		await Refresh();
		Position = columnInfo.Index;
	}

	[RelayCommand]
	async Task DropOnCard(CardInfo? card)
	{
		if (card is null) return;
		var columnInfo = Columns.FirstOrDefault(x => x.KanbanColumn.Cards.Contains(card.KanbanCard));
		if (columnInfo is null) return;
		await Drop(columnInfo);
	}

	[RelayCommand]
	void ItemDragOver(CardInfo card)
	{
		card.IsDragOver = true;
		Debug.WriteLine($"ItemDraggedOver : {card.KanbanCard.Name}");
	}

	[RelayCommand]
	void ItemDragLeave(CardInfo card)
	{
		card.IsDragOver = false;
		Debug.WriteLine($"ItemDraggedLeave : {card.KanbanCard.Name}");
	}

	[RelayCommand]
	void DragOver(string direction)
	{
		switch (Enum.Parse<SwipeDirection>(direction))
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
	void DragStarting(CardInfo card)
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

		var column = new KanbanColumn { Name = columnName, Wip = wip, Order = Columns.Count + 1 };
		await dbContext.Columns.AddAsync(column);
		await dbContext.SaveChangesAsync();
		await Refresh();
		await ToastAsync("Column is added");
	}

	[RelayCommand]
	async Task AddCard(int columnId)
	{
		var column = await dbContext.Columns.Include(column => column.Cards).FirstOrDefaultAsync(x => x.Id == columnId);
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
		await dbContext.Cards.AddAsync(new KanbanCard
		{
			Name = cardName,
			Description = cardDescription,
			ColumnId = columnId,
			Order = column.Cards.Count + 1
		});
		await dbContext.SaveChangesAsync();

		await Refresh();
		await ToastAsync("Card is added");
	}

	[RelayCommand]
	async Task DeleteCard(CardInfo? card)
	{
		if (card is null) return;
		var result = await AlertAsync("Delete card", $"Do you want to delete card \"{card.KanbanCard.Name}\"?");
		if (!result) return;

		await SnackbarAsync("The card is removed", "Cancel", async () =>
		{
			await ToastAsync("Task is cancelled");
			await dbContext.Cards.AddAsync(card.KanbanCard);
			await dbContext.SaveChangesAsync();
			await Refresh();
		});
		dbContext.Cards.Remove(card.KanbanCard);
		await dbContext.SaveChangesAsync();
		await Refresh();
	}

	[RelayCommand]
	async Task DeleteColumn(ColumnInfo? columnInfo)
	{
		if (columnInfo is null) return;
		var result = await AlertAsync("Delete column",
			$"Do you want to delete column \"{columnInfo.KanbanColumn.Name}\" and all its cards?");
		if (!result) return;

		await SnackbarAsync("The column is removed", "Cancel", async () =>
		{
			await dbContext.Columns.AddAsync(columnInfo.KanbanColumn);
			await dbContext.SaveChangesAsync();
			await Refresh();
		});

		await dbContext.Columns.Where(x => x.Id == columnInfo.KanbanColumn.Id).ExecuteDeleteAsync();
		await Refresh();
	}

	[RelayCommand]
	private async Task Refresh()
	{
		var items = await dbContext.Columns
								   .Include(x => x.Cards)
								   .OrderBy(x => x.Order)
								   .ToArrayAsync();
		Columns = items
			.Select(OrderCards)
			.ToObservableCollection();
		Position = 0;
	}

	private static ColumnInfo OrderCards(KanbanColumn c, int columnNumber)
	{
		c.Cards = c.Cards.OrderBy(card => card.Order).ToObservableCollection();
		return new ColumnInfo(columnNumber, c);
	}

	private static Task<bool> AlertAsync(string title, string message)
	{
		var currentPage = Application.Current?.Windows.LastOrDefault()?.Page;
		return currentPage is null ?
			Task.FromResult(false) :
			currentPage.DisplayAlert(title, message, "Yes", "No");
	}

	private static Task<string> UserPromptAsync(string title, string message, Keyboard keyboard)
	{
		var currentPage = Application.Current?.Windows.LastOrDefault()?.Page;
		return currentPage is null ?
			Task.FromResult(string.Empty) :
			currentPage.DisplayPromptAsync(title, message, keyboard: keyboard);
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

	private void UpdateCardsOrder(KanbanColumn kanbanColumn)
	{
		var cards = kanbanColumn.Cards.OrderBy(x => x.Order).ToList();
		for (int i = 0; i < cards.Count; i++)
		{
			kanbanColumn.Cards[i].Order = i;
		}
	}
}