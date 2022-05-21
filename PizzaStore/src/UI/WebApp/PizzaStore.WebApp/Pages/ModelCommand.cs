namespace PizzaStore.WebApp.Pages;

using System.Windows.Input;

public class ModelCommand<T> : ICommand
{
	private readonly Predicate<T?>? canExecute;

	private readonly Action<T?>? execute;

	public ModelCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
	{
		this.execute = execute;
		this.canExecute = canExecute;
	}

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object? parameter)
	{
		if (parameter is T parameterT)
		{
			return canExecute == null || canExecute(parameterT);
		}

		return false;
	}

	public void Execute(object? parameter)
	{
		if (parameter is T parameterT)
		{
			execute?.Invoke(parameterT);
		}
	}

	public void OnCanExecuteChanged()
	{
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}