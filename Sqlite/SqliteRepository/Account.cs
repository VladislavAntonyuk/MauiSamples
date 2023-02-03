namespace SqliteRepository;
using SQLite;

public class Account
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	public string Email { get; set; } = string.Empty;

	public decimal Balance { get; set; }
}