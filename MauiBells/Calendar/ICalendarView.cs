namespace MauiBells.Calendar;

public interface ICalendarView : IView
{
	DayOfWeek FirstDayOfWeek { get; }
	DateTimeOffset MinDate { get; }
	DateTimeOffset MaxDate { get; }
	DateTimeOffset? SelectedDate { get; set; }
	void OnSelectedDateChanged(DateTimeOffset? selectedDate);
}