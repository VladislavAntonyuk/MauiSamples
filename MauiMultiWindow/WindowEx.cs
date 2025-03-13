namespace MauiMultiWindow;

public class WindowEx(Page page) : Window(page)
{
	public bool IsActive { get; private set; }

	protected override void OnActivated()
	{
		base.OnActivated();
		IsActive = true;
	}

	protected override void OnDeactivated()
	{
		IsActive = false;
		base.OnDeactivated();
	}
}