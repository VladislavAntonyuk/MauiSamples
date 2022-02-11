using System.Collections.ObjectModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace KanbanBoard.Models;

public sealed class Column
{
    public Column()
    {
        Cards = new ObservableCollection<Card>();
    }

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }

    public int Wip { get; set; } = int.MaxValue;

    [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
    public ICollection<Card> Cards { get; set; }

    public int Order { get; set; }
}
