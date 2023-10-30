using task_list_app_maui.ViewModels;

namespace task_list_app_maui.Views;

public partial class EditarTarefaView : ContentPage
{
	private readonly TarefasViewModel tarefasViewModel;

	public EditarTarefaView(TarefasViewModel tarefasViewModel)
	{
		InitializeComponent();

		this.tarefasViewModel = tarefasViewModel;

		BindingContext = this.tarefasViewModel;

		this.tarefasViewModel.EditModeOn();
	}

	private async void OnSaveTaskClicked(object sender, EventArgs e)
	{
		tarefasViewModel.Edit();

		if (!string.IsNullOrEmpty(tarefasViewModel.HasErrorsCodeBehind))
		{
			await DisplayAlert(Title, tarefasViewModel.HasErrorsCodeBehind, "OK");
		}
		else
		{
			tarefasViewModel.GetAll();

			await Navigation.PopModalAsync();
		}
	}

	private async void OnCloseEditTaskClicked(object sender, EventArgs e)
	{
		tarefasViewModel.EditModeOff();

		await Navigation.PopModalAsync();
	}
}