namespace MauiBells.Calendar;

using Microsoft.Maui.Handlers;

public partial class CalendarHandler : ViewHandler<ICalendarView, CalendarView>
{
	protected override CalendarView CreatePlatformView()
	{
		return new CalendarView();
	}

	private static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView virtualView)
	{
		
	}
	private static void MapMinDate(CalendarHandler handler, ICalendarView virtualView)
	{
		
	}

	private static void MapMaxDate(CalendarHandler handler, ICalendarView virtualView)
	{

	}

	private static void MapSelectedDate(CalendarHandler handler, ICalendarView virtualView)
	{
		
	}
}