namespace MauiTaskListApp.Views;

using Models;
using ViewModels;

public partial class CompletedTasksView : ContentPage
{
	private readonly TasksViewModel tasksViewModel;

	public CompletedTasksView(TasksViewModel tasksViewModel)
	{
		InitializeComponent();

		BindingContext = this.tasksViewModel = tasksViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		this.tasksViewModel.Initialize();
	}

	private async void OnEditClicked(object sender, EventArgs e)
	{
		var imageButton = (ImageButton)sender;
		var task = (Task)imageButton.BindingContext;

		CollectionViewTarefasConcluidas.SelectedItem = task;

		if (CollectionViewTarefasConcluidas.SelectedItem == null) return;

		await Navigation.PushModalAsync(new EditTaskView(tasksViewModel));
	}

	private async void OnDeleteClicked(object sender, EventArgs e)
	{
		var imageButton = (ImageButton)sender;
		var task = (Task)imageButton.BindingContext;

		CollectionViewTarefasConcluidas.SelectedItem = task;

		if (CollectionViewTarefasConcluidas.SelectedItem == null) return;

		bool answer = await DisplayAlert(Title, "Do you confirm the deletion of this task?", "Yes", "No");
		if (!answer) return;

		tasksViewModel.DeleteCommand.Execute(null);

		if (!string.IsNullOrEmpty(tasksViewModel.HasErrorsCodeBehind))
		{
			await DisplayAlert(Title, tasksViewModel.HasErrorsCodeBehind, "OK");
		}
		else
		{
			tasksViewModel.GetAllCommand.Execute(null);
		}
	}
}