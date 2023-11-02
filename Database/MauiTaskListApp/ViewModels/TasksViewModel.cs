namespace MauiTaskListApp.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using Models;

public sealed partial class TasksViewModel(AppDbContext context) : ObservableObject, IDisposable
{
	public ObservableCollection<Task> ActiveTasks { get; } = new();

	public ObservableCollection<Task> FinishedTasks { get; } = new();

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
	private void Edit(Task task)
	{
		try
		{
			HasErrorsCodeBehind = string.Empty;

			context.Update(Task);
			context.SaveChanges();

			EditModeOn();
		}
		catch (Exception ex)
		{
			HasErrorsCodeBehind = ex.Message;
		}
	}

	[RelayCommand]
	private void Delete(Task task)
	{
		try
		{
			HasErrorsCodeBehind = string.Empty;

			context.Remove(task);
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
		foreach (var activeTask in tasks.Where(x => !x.IsFinished).OrderBy(x => x.Date))
		{
			ActiveTasks.Add(activeTask);
		}

		FinishedTasks.Clear();
		foreach (var activeTask in tasks.Where(x => x.IsFinished).OrderByDescending(x => x.Date))
		{
			FinishedTasks.Add(activeTask);
		}
	}

	public void Initialize()
	{
		GetAll();
	}

	public void Dispose() => context.Dispose();
}