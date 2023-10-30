using task_list_app_maui.Models;
using task_list_app_maui.ViewModels;

namespace task_list_app_maui.Views;

public partial class TarefasConcluidasView : ContentPage
{
	private readonly TarefasViewModel tarefasViewModel;

	public TarefasConcluidasView(TarefasViewModel tarefasViewModel)
	{
		InitializeComponent();

		this.tarefasViewModel = tarefasViewModel;
		this.tarefasViewModel.GetAll();

		BindingContext = this.tarefasViewModel;
	}

	private async void OnEditClicked(object sender, EventArgs e)
	{
		var imageButton = (ImageButton)sender;
		var tarefa = (Tarefa)imageButton.BindingContext;

		CollectionViewTarefasConcluidas.SelectedItem = tarefa;

		if (CollectionViewTarefasConcluidas.SelectedItem == null) return;

		await Navigation.PushModalAsync(new EditarTarefaView(tarefasViewModel));
	}

	private async void OnDeleteClicked(object sender, EventArgs e)
	{
		var imageButton = (ImageButton)sender;
		var tarefa = (Tarefa)imageButton.BindingContext;

		CollectionViewTarefasConcluidas.SelectedItem = tarefa;

		if (CollectionViewTarefasConcluidas.SelectedItem == null) return;

		bool answer = await DisplayAlert(Title, "Do you confirm the deletion of this task?", "Yes", "No");
		if (!answer) return;

		tarefasViewModel.Delete();

		if (!string.IsNullOrEmpty(tarefasViewModel.HasErrorsCodeBehind))
		{
			await DisplayAlert(Title, tarefasViewModel.HasErrorsCodeBehind, "OK");
		}
		else
		{
			tarefasViewModel.GetAll();
		}
	}
}