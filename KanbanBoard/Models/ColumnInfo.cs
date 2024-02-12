namespace KanbanBoard.Models;

using System.Collections.ObjectModel;

public class ColumnInfo(int index, Column column)
{
	public Column Column { get; } = column;
public ObservableCollection<CardInfo> Cards { get; } = column.Cards.Select(x => new CardInfo(x)).ToObservableCollection();
public int Index { get; } = index;

public bool IsWipReached => Column.Cards.Count >= Column.Wip;
}