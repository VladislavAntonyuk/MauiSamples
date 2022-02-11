using SQLite;
using SQLiteNetExtensions.Attributes;

namespace KanbanBoard.Models;

public sealed class Card
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Order { get; set; }

    [ForeignKey(typeof(Column))]
    public int ColumnId { get; set; }

    [ManyToOne]
    public Column Column { get; set; }
}
