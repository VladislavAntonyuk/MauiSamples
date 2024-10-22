namespace KanbanBoardDb;

using System.Collections.ObjectModel;

public sealed class KanbanColumn
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public int Wip { get; set; } = int.MaxValue;

	public ObservableCollection<KanbanCard> Cards { get; set; } = new();

	public int Order { get; set; }
}