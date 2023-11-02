namespace MauiTaskListApp.Views;

using ViewModels;

public partial class EditTaskView : ContentPage
{
	private readonly TasksViewModel tarefasViewModel;

	public EditTaskView(TasksViewModel tarefasViewModel)
	{
		InitializeComponent();

		this.tarefasViewModel = tarefasViewModel;

		BindingContext = this.tarefasViewModel;

		this.tarefasViewModel.EditModeOnCommand.Execute(null);
	}

	private async void OnSaveTaskClicked(object sender, EventArgs e)
	{
		tarefasViewModel.EditCommand.Execute(null);

		if (!string.IsNullOrEmpty(tarefasViewModel.HasErrorsCodeBehind))
		{
			await DisplayAlert(Title, tarefasViewModel.HasErrorsCodeBehind, "OK");
		}
		else
		{
			tarefasViewModel.GetAllCommand.Execute(null);

			await Navigation.PopModalAsync();
		}
	}

	private async void OnCloseEditTaskClicked(object sender, EventArgs e)
	{
		tarefasViewModel.EditModeOffCommand.Execute(null);

		await Navigation.PopModalAsync();
	}
}