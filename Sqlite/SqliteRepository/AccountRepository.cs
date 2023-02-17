namespace SqliteRepository;
using SQLite;

public class AccountRepository : IDisposable
{
	private readonly SQLiteConnection database;

	public AccountRepository(string dbName)
	{
		var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
		database = new SQLiteConnection(dbPath);
		database.CreateTable<Account>();
	}

	public List<Account> GetAccounts()
	{
		return database.Table<Account>().ToList();
	}

	public int CreateAccount(Account account)
	{
		return database.Insert(account);
	}

	public int UpdateAccount(Account account)
	{
		return database.Update(account);
	}

	public int DeleteAccount(Account account)
	{
		return database.Delete(account);
	}

	public List<Account> QueryAccountWithPositiveBalance()
	{
		return database.Query<Account>("SELECT * FROM Account WHERE Balance > 0");
	}

	public List<Account> LinqZeroBalance()
	{
		return database.Table<Account>().Where(a => a.Balance == 0).ToList();
	}

	public void Dispose()
	{
		database.Dispose();
	}
}