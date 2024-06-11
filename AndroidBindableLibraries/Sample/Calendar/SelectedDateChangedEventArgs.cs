namespace Sample.Calendar;

public class SelectedDateChangedEventArgs(DateTimeOffset? selectedDate) : EventArgs
{
	public DateTimeOffset? SelectedDate { get; } = selectedDate;
}