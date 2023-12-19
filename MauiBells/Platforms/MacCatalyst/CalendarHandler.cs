namespace MauiBells.Calendar;

using Microsoft.Maui.Handlers;
using UIKit;

public partial class CalendarHandler : ViewHandler<ICalendarView, UICalendarView>
{
	protected override UICalendarView CreatePlatformView()
	{
		return new UICalendarView();
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