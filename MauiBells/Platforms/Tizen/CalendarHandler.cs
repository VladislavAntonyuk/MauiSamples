namespace MauiBells.Calendar;

using Microsoft.Maui.Handlers;
using Tizen.NUI.BaseComponents.View;

public partial class CalendarHandler : ViewHandler<ICalendarView, Calendar>
{
	protected override Calendar CreatePlatformView()
	{
		return new Calendar();
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