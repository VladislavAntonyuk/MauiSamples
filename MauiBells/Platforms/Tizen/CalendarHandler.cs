namespace MauiBells.Calendar;

using Microsoft.Maui.Handlers;
using Tizen.NUI.BaseComponents;

public partial class CalendarHandler : ViewHandler<ICalendarView, Tizen.NUI.BaseComponents.View>
{
	protected override View CreatePlatformView()
	{
		// https://github.com/Samsung/Tizen-CSharp-Samples/blob/master/Mobile/Xamarin.Forms/CalendarComponent/src/CalendarComponent/CalendarComponent.Tizen.Mobile/Renderers/CalendarViewRenderer.cs
		throw new NotSupportedException("Tizen.NUI.BaseComponents doesn't contain CalendarControl. Only ElmaSharp.Calendar exist");
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