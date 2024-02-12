namespace MauiBank.Models;

public class TransactionGroup(DateTime dateTime, List<Transaction> transactions) : List<Transaction>(transactions)
{
	public DateOnly Date { get; } = new(dateTime.Year, dateTime.Month, dateTime.Day);
}