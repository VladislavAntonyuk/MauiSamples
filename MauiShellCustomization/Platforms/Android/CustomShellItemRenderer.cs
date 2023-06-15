using Android.OS;
using Android.Views;
using Microsoft.Maui.Controls.Platform.Compatibility;
using View = Android.Views.View;

namespace MauiShellCustomization;

class CustomShellItemRenderer : ShellItemRenderer
{
	public CustomShellItemRenderer(IShellContext context) : base(context)
	{

	}

	public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
	{
		return base.OnCreateView(inflater, container, savedInstanceState);
	}
}