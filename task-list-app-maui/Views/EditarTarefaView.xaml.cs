using task_list_app_maui.ViewModels;

namespace task_list_app_maui.Views;

public partial class EditarTarefaView : ContentPage
{
	private TarefasViewModel _tarefasViewModel;

	public EditarTarefaView(TarefasViewModel tarefasViewModel)
	{
		InitializeComponent();

		_tarefasViewModel = tarefasViewModel;

		BindingContext = _tarefasViewModel;

		_tarefasViewModel.EditModeOn();
	}

	private async void OnSaveTaskClicked(object sender, EventArgs e)
	{
		_tarefasViewModel.Edit();

		if (!string.IsNullOrEmpty(_tarefasViewModel.HasErrorsCodeBehind))
        {
            await DisplayAlert(Title, _tarefasViewModel.HasErrorsCodeBehind, "OK");
        }
        else
        {
			_tarefasViewModel.GetAll();

			await Navigation.PopModalAsync();
        }
    }

	private async void OnCloseEditTaskClicked(object sender, EventArgs e)
	{
		_tarefasViewModel.EditModeOff();

        await Navigation.PopModalAsync();
    }
}
