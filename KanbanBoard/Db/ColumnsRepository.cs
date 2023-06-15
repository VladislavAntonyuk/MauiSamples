namespace KanbanBoard.Db;
using Microsoft.EntityFrameworkCore;
using Models;

public class ColumnsRepository : IColumnsRepository
{
	private readonly KanbanBoardDbContext dbContext;

	public ColumnsRepository(KanbanBoardDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task DeleteItem(int id)
	{
		var column = await dbContext.Columns.SingleOrDefaultAsync(x => x.Id == id);
		if (column != null)
		{
			dbContext.Remove(column);
			await dbContext.SaveChangesAsync();
		}
	}

	public Task<Column?> GetItem(int id)
	{
		return dbContext.Columns.Include(x => x.Cards).FirstOrDefaultAsync(x => x.Id == id);
	}

	public Task<List<Column>> GetItems()
	{
		return dbContext.Columns.Include(x => x.Cards).ToListAsync();
	}

	public async Task<Column?> UpdateItem(Column item)
	{
		var column = await dbContext.Columns.SingleOrDefaultAsync(x => x.Id == item.Id);
		if (column == null)
		{
			return column;
		}

		column.Name = item.Name;
		column.Wip = item.Wip;
		column.Order = item.Order;

		await dbContext.SaveChangesAsync();
		return column;
	}

	public async Task<Column> SaveItem(Column item)
	{
		await dbContext.Columns.AddAsync(item);
		await dbContext.SaveChangesAsync();
		return item;
	}
}