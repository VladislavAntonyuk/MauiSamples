namespace KanbanBoard.Models;

using System.Collections.ObjectModel;
using KanbanBoardDb;

public class ColumnInfo(int index, KanbanColumn kanbanColumn)
{
	public KanbanColumn KanbanColumn { get; } = kanbanColumn;
	public ObservableCollection<CardInfo> Cards { get; } = kanbanColumn.Cards.Select(x => new CardInfo(x)).ToObservableCollection();
	public int Index { get; } = index;

	public bool IsWipReached => KanbanColumn.Cards.Count >= KanbanColumn.Wip;
}