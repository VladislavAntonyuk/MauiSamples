using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbanBoard.Db;
using KanbanBoard.Models;
using Application = Microsoft.Maui.Controls.Application;

namespace KanbanBoard;

public class MainPageViewModel : ObservableObject
{
    private ObservableCollection<ColumnInfo> _columns;
    private Card? _dragCard;
    private int _position;
    private readonly ICardsRepository _cardsRepository;
    private readonly IColumnsRepository _columnsRepository;

    public MainPageViewModel(ICardsRepository cardsRepository, IColumnsRepository columnsRepository)
    {
        _cardsRepository = cardsRepository;
        _columnsRepository = columnsRepository;
        _columns = new();
        RefreshCommand.Execute(null);
    }

    public ICommand RefreshCommand => new AsyncRelayCommand(UpdateCollection);

    public ICommand DropCommand => new AsyncRelayCommand<ColumnInfo>(async columnInfo =>
    {
        if (_dragCard is null || columnInfo is null || columnInfo.Column.Cards.Count >= columnInfo.Column.Wip) return;

        var cardToUpdate = await _cardsRepository.GetItem(_dragCard.Id);
        if (cardToUpdate is not null)
        {
            cardToUpdate.ColumnId = columnInfo.Column.Id;
            await _cardsRepository.UpdateItem(cardToUpdate);
        }

        await UpdateCollection();
        Position = columnInfo.Index;
    });

    public ICommand DragStartingCommand => new RelayCommand<Card>(card => { _dragCard = card; });

    public ICommand DropCompletedCommand => new RelayCommand(() => { _dragCard = null; });

    public ICommand AddColumn => new AsyncRelayCommand(async () =>
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

        var column = new Column { Name = columnName, Wip = wip, Order = _columns.Count + 1 };
        await _columnsRepository.SaveItem(column);
        await UpdateCollection();
        await ToastAsync("Column is added");
    });

    public ICommand AddCard => new AsyncRelayCommand<int>(async columnId =>
    {
        var column = await _columnsRepository.GetItem(columnId);
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
        await _cardsRepository.SaveItem(new Card
        {
            Name = cardName,
            Description = cardDescription,
            ColumnId = columnId,
            Order = column.Cards.Count + 1
        });

        await UpdateCollection();
        await ToastAsync("Card is added");
    });

    public ICommand DeleteCard => new AsyncRelayCommand<Card>(async card =>
    {
        if (card is null) return;
        var result = await AlertAsync("Delete card", $"Do you want to delete card \"{card.Name}\"?");
        if (!result) return;

        await SnackbarAsync("The card is removed", "Cancel", async () =>
        {
            await ToastAsync("Task is cancelled");
            await _cardsRepository.SaveItem(card);
            await UpdateCollection();
        });
        await _cardsRepository.DeleteItem(card.Id);
        await UpdateCollection();
    });

    public ICommand DeleteColumn => new AsyncRelayCommand<ColumnInfo>(async columnInfo =>
    {
        if (columnInfo is null) return;
        var result = await AlertAsync("Delete column",
            $"Do you want to delete column \"{columnInfo.Column.Name}\" and all its cards?");
        if (!result) return;

        await SnackbarAsync("The column is removed", "Cancel", async () =>
        {
            await _columnsRepository.SaveItem(columnInfo.Column);
            await UpdateCollection();
        });

        await _columnsRepository.DeleteItem(columnInfo.Column.Id);
        await UpdateCollection();
    });

    public ObservableCollection<ColumnInfo> Columns
    {
        get => _columns;
        set => SetProperty(ref _columns, value);
    }

    public int Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }

    private async Task UpdateCollection()
    {
        var items = await _columnsRepository.GetItems();
        Columns = items
            .OrderBy(c => c.Order)
            .ToList()
            .Select(OrderCards)
            .ToObservableCollection();
        Position = 0;
    }

    private static ColumnInfo OrderCards(Column c, int columnNumber)
    {
        c.Cards = c.Cards.OrderBy(card => card.Order).ToList();
        return new ColumnInfo(columnNumber, c);
    }

    private static Task<bool> AlertAsync(string title, string message)
    {
        if (Application.Current?.MainPage is null) return Task.FromResult(false);
        return Application.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
    }

    private static Task<string> UserPromptAsync(string title, string message, Keyboard keyboard)
    {
        if (Application.Current?.MainPage is null) return Task.FromResult(string.Empty);
        return Application.Current.MainPage.DisplayPromptAsync(title, message, keyboard: keyboard);
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
}
