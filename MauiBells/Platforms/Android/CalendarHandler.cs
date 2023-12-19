namespace MauiBells.Calendar;

using Microsoft.Maui.Handlers;
using Calendar = Android.Widget.CalendarView;

public partial class CalendarHandler : ViewHandler<ICalendarView, Calendar>
{
	protected override Calendar CreatePlatformView()
	{
		return new Calendar(Context);
	}
	protected override void ConnectHandler(Calendar platformView)
	{
		base.ConnectHandler(platformView);
		platformView.DateChange += PlatformView_SelectedDatesChanged;
	}

	private void PlatformView_SelectedDatesChanged(object? sender, Calendar.DateChangeEventArgs e)
	{
		PlatformView.DateChange -= PlatformView_SelectedDatesChanged;
		VirtualView.SelectedDate = new DateTime(e.Year, e.Month + 1, e.DayOfMonth, 0, 0, 0);
		VirtualView.OnSelectedDateChanged(VirtualView.SelectedDate);
		PlatformView.DateChange += PlatformView_SelectedDatesChanged;
	}

	protected override void DisconnectHandler(Calendar platformView)
	{
		platformView.DateChange -= PlatformView_SelectedDatesChanged;
		base.DisconnectHandler(platformView);
	}

	private static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.FirstDayOfWeek = (int)virtualView.FirstDayOfWeek;
	}

	private static void MapMinDate(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.MinDate = virtualView.MinDate.ToUnixTimeMilliseconds();
	}

	private static void MapMaxDate(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.MaxDate = virtualView.MaxDate.ToUnixTimeMilliseconds();
	}

	private static void MapSelectedDate(CalendarHandler handler, ICalendarView virtualView)
	{
		if (virtualView.SelectedDate is null)
		{
			return;
		}

		handler.PlatformView.SetDate(virtualView.SelectedDate.Value.ToUnixTimeMilliseconds(), true, true);
	}
}