namespace MauiBells.Calendar;

using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using Windows.Globalization;
using Calendar = Microsoft.UI.Xaml.Controls.CalendarView;

public partial class CalendarHandler : ViewHandler<ICalendarView, Calendar>
{
	protected override Calendar CreatePlatformView()
	{
		return new Calendar();
	}

	protected override void ConnectHandler(Calendar platformView)
	{
		base.ConnectHandler(platformView);
		platformView.SelectedDatesChanged += PlatformView_SelectedDatesChanged;
	}

	private void PlatformView_SelectedDatesChanged(Calendar sender, CalendarViewSelectedDatesChangedEventArgs args)
	{
		PlatformView.SelectedDatesChanged -= PlatformView_SelectedDatesChanged;
		if (args.AddedDates.Count == 0)
		{
			VirtualView.SelectedDate = null;
		}

		if (args.AddedDates.Count > 0)
		{
			VirtualView.SelectedDate = args.AddedDates[0];
		}

		VirtualView.OnSelectedDateChanged(VirtualView.SelectedDate);
		PlatformView.SelectedDatesChanged += PlatformView_SelectedDatesChanged;
	}

	protected override void DisconnectHandler(Calendar platformView)
	{
		platformView.SelectedDatesChanged -= PlatformView_SelectedDatesChanged;
		base.DisconnectHandler(platformView);
	}

	private static void MapFirstDayOfWeek(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.FirstDayOfWeek = (DayOfWeek)virtualView.FirstDayOfWeek;
	}

	private static void MapMinDate(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.MinDate = virtualView.MinDate;
	}

	private static void MapMaxDate(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.MaxDate = virtualView.MaxDate;
	}

	private static void MapSelectedDate(CalendarHandler handler, ICalendarView virtualView)
	{
		handler.PlatformView.SelectedDates.Clear();
		if (virtualView.SelectedDate is not null)
		{
			handler.PlatformView.SelectedDates.Add(virtualView.SelectedDate.Value);
			handler.PlatformView.SetDisplayDate(virtualView.SelectedDate.Value);
		}
	}
}