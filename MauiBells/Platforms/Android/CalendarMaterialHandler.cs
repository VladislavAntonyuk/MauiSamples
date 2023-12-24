namespace MauiBells.Calendar;

using System.ComponentModel;
using Android;
using Com.Applandeo.Materialcalendarview;
using Com.Applandeo.Materialcalendarview.Listeners;
using Java.Lang;
using Microsoft.Maui.Handlers;
using Calendar = Com.Applandeo.Materialcalendarview.CalendarView;

public class CalendarMaterialHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : ViewHandler<ICalendarView, Calendar>(mapper, commandMapper)
{
	public static IPropertyMapper<ICalendarView, CalendarMaterialHandler> PropertyMapper = new PropertyMapper<ICalendarView, CalendarMaterialHandler>(ViewMapper)
	{
		[nameof(ICalendarView.FirstDayOfWeek)] = MapFirstDayOfWeek,
		[nameof(ICalendarView.MinDate)] = MapMinDate,
		[nameof(ICalendarView.MaxDate)] = MapMaxDate,
		[nameof(ICalendarView.SelectedDate)] = MapSelectedDate,
	};

	public static CommandMapper<ICalendarView, CalendarMaterialHandler> CommandMapper = new(ViewCommandMapper);

	public CalendarMaterialHandler() : this(PropertyMapper, CommandMapper)
	{
	}

	protected override Calendar CreatePlatformView()
	{
		return new Calendar(Context);
	}

	protected override void ConnectHandler(Calendar platformView)
	{
		base.ConnectHandler(platformView);
		platformView.CalendarDayClick += PlatformView_SelectedDatesChanged;
	}

	private void PlatformView_SelectedDatesChanged(object? sender, CalendarDayClickEventArgs e)
	{
		var calendar = e.CalendarDay.Calendar;
		var time = TimeSpan.FromMilliseconds(calendar.TimeInMillis);
		var result = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		VirtualView.SelectedDate = new DateTimeOffset(result.Add(time).Add(TimeSpan.FromMilliseconds(calendar.TimeZone.RawOffset)));
		VirtualView.OnSelectedDateChanged(VirtualView.SelectedDate);
	}

	protected override void DisconnectHandler(Calendar platformView)
	{
		platformView.CalendarDayClick -= PlatformView_SelectedDatesChanged;
		base.DisconnectHandler(platformView);
	}

	private static void MapFirstDayOfWeek(CalendarMaterialHandler handler, ICalendarView virtualView)
	{
		var calendarWeekDate = virtualView.FirstDayOfWeek switch
		{
			DayOfWeek.Sunday => CalendarWeekDay.Sunday,
			DayOfWeek.Monday => CalendarWeekDay.Monday,
			DayOfWeek.Tuesday => CalendarWeekDay.Tuesday,
			DayOfWeek.Wednesday => CalendarWeekDay.Wednesday,
			DayOfWeek.Thursday => CalendarWeekDay.Thursday,
			DayOfWeek.Friday => CalendarWeekDay.Friday,
			DayOfWeek.Saturday => CalendarWeekDay.Saturday,
			_ => throw new InvalidEnumArgumentException(),
		};
		ArgumentNullException.ThrowIfNull(calendarWeekDate);
		handler.PlatformView.SetFirstDayOfWeek(calendarWeekDate);
	}

	private static void MapMinDate(CalendarMaterialHandler handler, ICalendarView virtualView)
	{
		var calendar = Java.Util.Calendar.Instance;
		calendar.Set(virtualView.MinDate.Year, virtualView.MinDate.Month - 1, virtualView.MinDate.Day);
		handler.PlatformView.SetMinimumDate(calendar);
	}

	private static void MapMaxDate(CalendarMaterialHandler handler, ICalendarView virtualView)
	{
		var calendar = Java.Util.Calendar.Instance;
		calendar.Set(virtualView.MaxDate.Year, virtualView.MaxDate.Month - 1, virtualView.MaxDate.Day);
		handler.PlatformView.SetMaximumDate(calendar);
	}

	private static void MapSelectedDate(CalendarMaterialHandler handler, ICalendarView virtualView)
	{
		if (virtualView.SelectedDate is null)
		{
			return;
		}

		var calendar = Java.Util.Calendar.Instance;
		calendar.Set(virtualView.SelectedDate.Value.Year, virtualView.SelectedDate.Value.Month - 1, virtualView.SelectedDate.Value.Day);
		List<CalendarDay> events = new();
		var day = new CalendarDay(calendar);
		day.BackgroundResource = Integer.ValueOf(Resource.Color.HoloRedDark);
		events.Add(day);
		handler.PlatformView.SetCalendarDays(events);
	}
}