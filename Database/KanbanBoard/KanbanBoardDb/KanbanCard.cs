namespace KanbanBoardDb;

public sealed class KanbanCard
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public int Order { get; set; }

	public int ColumnId { get; set; }

	public KanbanColumn? Column { get; set; }
}