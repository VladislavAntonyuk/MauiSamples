namespace Com.Skydoves.Balloon.Databinding;

using Android.Runtime;

public sealed partial class BalloonLayoutBodyBinding : global::Java.Lang.Object, global::AndroidX.ViewBinding.IViewBinding
{
	public unsafe global::Android.Views.View Root
	{
		// Metadata.xml XPath method reference: path="/api/package[@name='com.skydoves.balloon.databinding']/class[@name='BalloonLayoutOverlayBinding']/method[@name='getRoot' and count(parameter)=0]"
		[Register("getRoot", "()Lcom/skydoves/balloon/overlay/BalloonAnchorOverlayView;", "")]
		get
		{
			const string __id = "getRoot.()Lcom/skydoves/balloon/overlay/BalloonAnchorOverlayView;";
			try
			{
				var __rm = _members.InstanceMethods.InvokeAbstractObjectMethod(__id, this, null);
				return global::Java.Lang.Object.GetObject<global::Com.Skydoves.Balloon.Overlay.BalloonAnchorOverlayView>(__rm.Handle, JniHandleOwnership.TransferLocalRef)!;
			}
			finally
			{
			}
		}
	}
}