using task_list_app_maui.Models;
using task_list_app_maui.ViewModels;

namespace task_list_app_maui.Views;

public partial class TarefasConcluidasView : ContentPage
{
	private TarefasViewModel _tarefasViewModel;

	public TarefasConcluidasView(TarefasViewModel tarefasViewModel)
	{
		InitializeComponent();

		_tarefasViewModel = tarefasViewModel;
		_tarefasViewModel.GetAll();

		BindingContext = _tarefasViewModel;
	}

	private async void OnEditClicked(object sender, EventArgs e)
	{
		var imageButton = (ImageButton)sender;
    	var tarefa = (Tarefa)imageButton.BindingContext;

		CollectionViewTarefasConcluidas.SelectedItem = tarefa;

		if (CollectionViewTarefasConcluidas.SelectedItem == null) return;

		await Navigation.PushModalAsync(new EditarTarefaView(_tarefasViewModel));
	}

	private async void OnDeleteClicked(object sender, EventArgs e)
	{
		var imageButton = (ImageButton)sender;
    	var tarefa = (Tarefa)imageButton.BindingContext;

		CollectionViewTarefasConcluidas.SelectedItem = tarefa;

		if (CollectionViewTarefasConcluidas.SelectedItem == null) return;

		bool answer = await DisplayAlert(Title, "Do you confirm the deletion of this task?", "Yes", "No");
		if (!answer) return;

		_tarefasViewModel.Delete();

		if (!string.IsNullOrEmpty(_tarefasViewModel.HasErrorsCodeBehind))
        {
            await DisplayAlert(Title, _tarefasViewModel.HasErrorsCodeBehind, "OK");
        }
        else
        {
			_tarefasViewModel.GetAll();
        }
	}
}
