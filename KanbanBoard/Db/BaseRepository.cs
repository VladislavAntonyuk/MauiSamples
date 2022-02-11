using SQLite;

namespace KanbanBoard.Db;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : new()
{
    private readonly Lazy<SQLiteConnection> _databaseConnectionHolder;
    protected SQLiteConnection Database
    {
        get
        {
            var database = _databaseConnectionHolder.Value;
            database.CreateTable<T>();
            return database;
        }
    }

    protected BaseRepository(IPath path)
    {
        var dbPath = path.GetDatabasePath();
        _databaseConnectionHolder = new Lazy<SQLiteConnection>(() => new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));
    }

    public virtual Task<List<T>> GetItems()
    {
        return Task.FromResult(Database.Table<T>().ToList());
    }

    public Task<T> GetItem(int id)
    {
        return Task.FromResult(Database.Get<T>(id));
    }

    public Task DeleteItem(int id)
    {
        Database.Delete<T>(id);
        return Task.CompletedTask;
    }

    public Task<T> UpdateItem(T item)
    {
        Database.Update(item);
        return Task.FromResult(item);
    }

    public Task<T> SaveItem(T item)
    {
        Database.Insert(item);
        return Task.FromResult(item);
    }
}
