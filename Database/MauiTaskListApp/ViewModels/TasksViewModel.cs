namespace MauiTaskListApp.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using Models;

public sealed partial class TasksViewModel(AppDbContext context) : ObservableObject, IDisposable
{
	public ObservableCollection<Task> ActiveTasks { get; set; } = [];
	public ObservableCollection<Task> FinishedTasks { get; set; } = [];

	[ObservableProperty]
	private Task task = new();

	public Task TaskEditMode { get; set; } = new();

	[ObservableProperty]
	private string hasErrorsCodeBehind = string.Empty;

	[RelayCommand]
	private void New()
	{
		Task = new()
		{
			Date = DateTime.Now
		};
	}

	[RelayCommand]
	private void EditModeOn()
	{
		TaskEditMode.Id = Task.Id;
		TaskEditMode.Description = Task.Description;
		TaskEditMode.Date = Task.Date;
		TaskEditMode.IsFinished = Task.IsFinished;
	}

	[RelayCommand]
	private void EditModeOff()
	{
		Task.Id = TaskEditMode.Id;
		Task.Description = TaskEditMode.Description;
		Task.Date = TaskEditMode.Date;
		Task.IsFinished = TaskEditMode.IsFinished;
	}

	[RelayCommand]
	private void Create()
	{
		try
		{
			HasErrorsCodeBehind = string.Empty;

			context.Add(Task);
			context.SaveChanges();
		}
		catch (Exception ex)
		{
			HasErrorsCodeBehind = ex.Message;
		}
	}

	[RelayCommand]
	private void Edit()
	{
		try
		{
			HasErrorsCodeBehind = string.Empty;

			context.SaveChanges();

			EditModeOn();
		}
		catch (Exception ex)
		{
			HasErrorsCodeBehind = ex.Message;
		}
	}

	[RelayCommand]
	private void Delete()
	{
		try
		{
			HasErrorsCodeBehind = string.Empty;

			context.Remove(Task);
			context.SaveChanges();
		}
		catch (Exception ex)
		{
			HasErrorsCodeBehind = ex.Message;
		}
	}

	[RelayCommand]
	private void GetAll()
	{
		var tasks = context.Tasks.ToList();

		ActiveTasks.Clear();

		foreach (var unfinishedTask in tasks.Where(x => !x.IsFinished).OrderBy(x => x.Date))
		{
			ActiveTasks.Add(unfinishedTask);
		}

		FinishedTasks.Clear();

		foreach (var finishedTask in tasks.Where(x => x.IsFinished).OrderByDescending(x => x.Date))
		{
			FinishedTasks.Add(finishedTask);
		}
	}

	public void Dispose() => context.Dispose();
}