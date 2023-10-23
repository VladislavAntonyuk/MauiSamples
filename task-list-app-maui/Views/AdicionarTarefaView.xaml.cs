using task_list_app_maui.ViewModels;

namespace task_list_app_maui.Views;

public partial class AdicionarTarefaView : ContentPage
{
	private readonly TarefasViewModel tarefasViewModel;

	public AdicionarTarefaView(TarefasViewModel tarefasViewModel)
	{
		InitializeComponent();

		this.tarefasViewModel = tarefasViewModel;

		BindingContext = this.tarefasViewModel;
	}

	private async void OnSaveTaskClicked(object sender, EventArgs e)
	{
		tarefasViewModel.Create();

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

	private async void OnCloseAddTaskClicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}
