namespace KanbanBoard.Db;

public interface IBaseRepository<T> where T : new()
{
	Task DeleteItem(int id);
	Task<T?> GetItem(int id);
	Task<List<T>> GetItems();
	Task<T?> UpdateItem(T item);
	Task<T> SaveItem(T item);
}