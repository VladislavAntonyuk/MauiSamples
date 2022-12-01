namespace MauiBadge;

public partial class MainPage : ContentPage
{
	int count;
	private readonly INotificationCounter notificationCounter;

	public MainPage(INotificationCounter notificationCounter)
	{
		InitializeComponent();
		this.notificationCounter = notificationCounter;
	}

	private void OnIncrementClicked(object? sender, EventArgs args)
	{
		count++;
		CountLabel.Text = $"Count: {count}";
		notificationCounter.SetNotificationCount(count);
	}
	private void OnDecrementClicked(object? sender, EventArgs args)
	{
		count--;
		CountLabel.Text = $"Count: {count}";
		notificationCounter.SetNotificationCount(count);
	}
}