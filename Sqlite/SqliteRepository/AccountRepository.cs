namespace SqliteRepository;
using SQLite;

public class AccountRepository
{
    private readonly SQLiteConnection _database;

    public AccountRepository(string dbName)
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
        _database = new SQLiteConnection(dbPath);
        _database.CreateTable<Account>();
    }

    public List<Account> GetAccounts()
    {
        return _database.Table<Account>().ToList();
    }

    public int CreateAccount(Account account)
    {
        return _database.Insert(account);
    }

    public int UpdateAccount(Account account)
    {
        return _database.Update(account);
    }

    public int DeleteAccount(Account account)
    {
        return _database.Delete(account);
    }

    public List<Account> QueryAccountWithPositiveBalance()
    {
        return _database.Query<Account>("SELECT * FROM Account WHERE Balance > 0");
    }

    public List<Account> LinqZeroBalance()
    {
        return _database.Table<Account>().Where(a => a.Balance == 0).ToList();
    }
}