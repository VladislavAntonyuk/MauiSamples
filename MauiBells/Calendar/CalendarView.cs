namespace MauiBells.Calendar;

using System.ComponentModel;

public class CalendarView : View, ICalendarView
{
	readonly WeakEventManager calendarViewEventManager = new();

	public static readonly BindableProperty FirstDayOfWeekProperty = BindableProperty.Create(nameof(FirstDayOfWeek), typeof(DayOfWeek), typeof(CalendarView), default(DayOfWeek));
	public static readonly BindableProperty MinDateProperty = BindableProperty.Create(nameof(MinDate), typeof(DateTimeOffset), typeof(CalendarView), DateTimeOffset.MinValue);
	public static readonly BindableProperty MaxDateProperty = BindableProperty.Create(nameof(MaxDate), typeof(DateTimeOffset), typeof(CalendarView), DateTimeOffset.MaxValue);
	public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTimeOffset?), typeof(CalendarView));

	public DayOfWeek FirstDayOfWeek
	{
		get => (DayOfWeek)GetValue(FirstDayOfWeekProperty);
		set => SetValue(FirstDayOfWeekProperty, value);
	}

	[TypeConverter(typeof(DateTimeOffsetStringConverter))]
	public DateTimeOffset MinDate
	{
		get => (DateTimeOffset)GetValue(MinDateProperty);
		set => SetValue(MinDateProperty, value);
	}

	[TypeConverter(typeof(DateTimeOffsetStringConverter))]
	public DateTimeOffset MaxDate
	{
		get => (DateTimeOffset)GetValue(MaxDateProperty);
		set => SetValue(MaxDateProperty, value);
	}

	[TypeConverter(typeof(DateTimeOffsetStringConverter))]
	public DateTimeOffset? SelectedDate
	{
		get => (DateTimeOffset?)GetValue(SelectedDateProperty);
		set => SetValue(SelectedDateProperty, value);
	}

	public event EventHandler<SelectedDateChangedEventArgs> SelectedDateChanged
	{
		add => calendarViewEventManager.AddEventHandler(value);
		remove => calendarViewEventManager.RemoveEventHandler(value);
	}

	void ICalendarView.OnSelectedDateChanged(DateTimeOffset? selectedDate)
	{
		calendarViewEventManager.HandleEvent(this, new SelectedDateChangedEventArgs(selectedDate), nameof(SelectedDateChanged));
	}
}