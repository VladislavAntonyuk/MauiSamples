using KanbanBoard.Models;

namespace KanbanBoard.Db;

public interface IColumnsRepository : IBaseRepository<Column>
{
    public Task DeleteColumnWithCards(Column column);
}
