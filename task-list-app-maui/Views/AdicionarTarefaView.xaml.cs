using task_list_app_maui.ViewModels;

namespace task_list_app_maui.Views;

public partial class AdicionarTarefaView : ContentPage
{
	private TarefasViewModel _tarefasViewModel;

	public AdicionarTarefaView(TarefasViewModel tarefasViewModel)
	{
		InitializeComponent();

		_tarefasViewModel = tarefasViewModel;

		BindingContext = _tarefasViewModel;
	}

	private async void OnSaveTaskClicked(object sender, EventArgs e)
	{
		_tarefasViewModel.Create();

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

	private async void OnCloseAddTaskClicked(object sender, EventArgs e)
	{
        await Navigation.PopModalAsync();
    }
}
