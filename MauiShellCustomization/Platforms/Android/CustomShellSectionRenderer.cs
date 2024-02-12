namespace MauiShellCustomization;

using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using Microsoft.Maui.Controls.Platform.Compatibility;

internal class CustomShellSectionRenderer(IShellContext shellContext) : ShellSectionRenderer(shellContext)
{
	public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
	{
		var relativeLayout = new RelativeLayout(Context);
		relativeLayout.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
																		  ViewGroup.LayoutParams.MatchParent);

		var view = base.OnCreateView(inflater, container, savedInstanceState);
		if (view is not CoordinatorLayout coordinatorLayout)
		{
			return view;
		}

		for (var i = 0; i < coordinatorLayout.ChildCount; i++)
		{
			var child = coordinatorLayout.GetChildAt(i);
			coordinatorLayout.RemoveView(child);
			relativeLayout.AddView(child);
		}

		coordinatorLayout.AddView(relativeLayout);
		return coordinatorLayout;
	}
}