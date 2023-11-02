namespace MauiTaskListApp.Views;

using ViewModels;

public partial class AddTaskView : ContentPage
{
	private readonly TasksViewModel tasksViewModel;

	public AddTaskView(TasksViewModel tasksViewModel)
	{
		InitializeComponent();

		this.tasksViewModel = tasksViewModel;

		BindingContext = this.tasksViewModel;
	}

	private async void OnSaveTaskClicked(object sender, EventArgs e)
	{
		tasksViewModel.CreateCommand.Execute(null);

		if (!string.IsNullOrEmpty(tasksViewModel.HasErrorsCodeBehind))
		{
			await DisplayAlert(Title, tasksViewModel.HasErrorsCodeBehind, "OK");
		}
		else
		{
			tasksViewModel.GetAllCommand.Execute(null);

			await Navigation.PopModalAsync();
		}
	}

	private async void OnCloseAddTaskClicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}