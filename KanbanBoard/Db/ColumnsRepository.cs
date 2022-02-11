using KanbanBoard.Models;
using SQLiteNetExtensions.Extensions;

namespace KanbanBoard.Db;

public class ColumnsRepository : BaseRepository<Column>, IColumnsRepository
{
    public ColumnsRepository(IPath path) : base(path)
    {
    }

    public override Task<List<Column>> GetItems()
    {
        var columns = Database.Table<Column>().ToList();
        var cards = Database.Table<Card>().ToList();
        var result = columns.GroupJoin(cards, column => column.Id, card => card.ColumnId,
                    (column, columnCards) =>
                    {
                        column.Cards = columnCards.ToObservableCollection();
                        return column;
                    }).ToList();
        return Task.FromResult(result);
    }

    public Task DeleteColumnWithCards(Column column)
    {
        Database.Delete(column, true);
        return Task.CompletedTask;

    }
}
