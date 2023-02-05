namespace KanbanBoard.Models;

public class ColumnInfo
{
	public ColumnInfo(int index, Column column)
	{
		Index = index;
		Column = column;
	}

	public Column Column { get; }
	public int Index { get; }

	public bool IsWipReached => Column.Cards.Count >= Column.Wip;
}