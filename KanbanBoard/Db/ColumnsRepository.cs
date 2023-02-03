namespace KanbanBoard.Db;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;

public class ColumnsRepository : IColumnsRepository
{
	private readonly KanbanBoardDbContext _dbContext;

	public ColumnsRepository(KanbanBoardDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task DeleteItem(int id)
	{
		var column = await _dbContext.Columns.SingleOrDefaultAsync(x => x.Id == id);
		if (column != null)
		{
			_dbContext.Remove(column);
			await _dbContext.SaveChangesAsync();
		}
	}

	public Task<Column?> GetItem(int id)
	{
		return _dbContext.Columns.Include(x => x.Cards).FirstOrDefaultAsync(x => x.Id == id);
	}

	public Task<List<Column>> GetItems()
	{
		return _dbContext.Columns.Include(x => x.Cards).ToListAsync();
	}

	public async Task<Column?> UpdateItem(Column item)
	{
		var column = await _dbContext.Columns.SingleOrDefaultAsync(x => x.Id == item.Id);
		if (column == null)
		{
			return column;
		}

		column.Name = item.Name;
		column.Wip = item.Wip;
		column.Order = item.Order;

		await _dbContext.SaveChangesAsync();
		return column;
	}

	public async Task<Column> SaveItem(Column item)
	{
		await _dbContext.Columns.AddAsync(item);
		await _dbContext.SaveChangesAsync();
		return item;
	}
}