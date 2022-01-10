using SQLite;

namespace SqliteRepository;

public class Account
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public decimal Balance { get; set; }
}