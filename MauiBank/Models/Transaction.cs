namespace MauiBank.Models;

public class Transaction
{
	public DateTime DateTime { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Sum { get; set; }
}