using SQLite;

namespace MauiSqlite;

public class Account
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
}