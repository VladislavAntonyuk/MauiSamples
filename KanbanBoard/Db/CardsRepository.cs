using KanbanBoard.Models;

namespace KanbanBoard.Db;

public class CardsRepository : BaseRepository<Card>, ICardsRepository
{
    public CardsRepository(IPath path) : base(path)
    {

    }
}
