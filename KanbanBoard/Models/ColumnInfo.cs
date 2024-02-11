namespace KanbanBoard.Models;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public class ColumnInfo(int index, Column column)
{
	public Column Column { get; } = column;
public ObservableCollection<CardInfo> Cards { get; } = column.Cards.Select(x => new CardInfo(x)).ToObservableCollection();
public int Index { get; } = index;

public bool IsWipReached => Column.Cards.Count >= Column.Wip;
}

public partial class CardInfo(Card card) : ObservableObject
{
	[ObservableProperty]
	private bool isDragOver;

	public Card Card { get; } = card;
}