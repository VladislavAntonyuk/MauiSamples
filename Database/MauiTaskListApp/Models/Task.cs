namespace MauiTaskListApp.Models;

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

public class Task : ObservableValidator
{
	private int id;
	private string description = string.Empty;
	private DateTime date;
	private bool isFinished;

	[Key]
	[Required]
	public int Id { get => id; set => SetProperty(ref id, value, true); }

	[Required]
	[MaxLength(60)]
	public string Description { get => description; set => SetProperty(ref description, value, true); }

	[Required]
	public DateTime Date { get => date; set => SetProperty(ref date, value, true); }

	[Required]
	public bool IsFinished { get => isFinished; set => SetProperty(ref isFinished, value, true); }
}