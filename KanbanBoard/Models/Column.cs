namespace KanbanBoard.Models;
using System.Collections.ObjectModel;

public sealed class Column
{
	public Column()
	{
		Cards = new ObservableCollection<Card>();
		Name = string.Empty;
	}

	public int Id { get; set; }

	public string Name { get; set; }

	public int Wip { get; set; } = int.MaxValue;

	public ObservableCollection<Card> Cards { get; set; }

	public int Order { get; set; }
}