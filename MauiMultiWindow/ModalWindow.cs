namespace MauiMultiWindow;

public class ModalWindow<T>
{
	public Func<Task<T?>> SubmitContentAction { get; set; } = () => Task.FromResult<T?>(default);

	public string SubmitContent { get; set; } = "OK";

	public string CancelContent { get; set; } = "Cancel";

	public VisualElement Content { get; set; } = new ContentView();
}