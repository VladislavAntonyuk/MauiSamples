namespace KanbanBoard.Db;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;

public class CardsRepository : ICardsRepository
{
	private readonly KanbanBoardDbContext _dbContext;

	public CardsRepository(KanbanBoardDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task DeleteItem(int id)
	{
		var card = await _dbContext.Cards.SingleOrDefaultAsync(x => x.Id == id);
		if (card != null)
		{
			_dbContext.Remove(card);
			await _dbContext.SaveChangesAsync();
		}
	}

	public Task<Card?> GetItem(int id)
	{
		return _dbContext.Cards.Include(x => x.Column).FirstOrDefaultAsync(x => x.Id == id);
	}

	public Task<List<Card>> GetItems()
	{
		return _dbContext.Cards.ToListAsync();
	}

	public async Task<Card?> UpdateItem(Card item)
	{
		var card = await _dbContext.Cards.SingleOrDefaultAsync(x => x.Id == item.Id);
		if (card == null)
		{
			return card;
		}

		card.Description = item.Description;
		card.Name = item.Name;
		card.ColumnId = item.ColumnId;
		card.Order = item.Order;

		await _dbContext.SaveChangesAsync();
		return card;
	}

	public async Task<Card> SaveItem(Card item)
	{
		await _dbContext.Cards.AddAsync(item);
		await _dbContext.SaveChangesAsync();
		return item;
	}
}