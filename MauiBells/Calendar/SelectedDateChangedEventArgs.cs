namespace MauiBells.Calendar;

public class SelectedDateChangedEventArgs(DateTimeOffset? selectedDate) : EventArgs
{
	public DateTimeOffset? SelectedDate { get; } = selectedDate;
}