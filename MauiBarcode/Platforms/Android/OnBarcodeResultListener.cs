namespace MauiBarcode;

using Android.Gms.Tasks;
using Android.Runtime;
using Java.Lang;
using Xamarin.Google.MLKit.Vision.Barcode.Common;

public class OnBarcodeResultListener(TaskCompletionSource<Barcode?> taskCompletionSource) : Object, IOnCompleteListener
{
	public void OnComplete(Task task)
	{
		if (task.IsSuccessful)
		{
			taskCompletionSource.SetResult(task.Result.JavaCast<Barcode>());
		}
		else if (task.IsCanceled)
		{
			taskCompletionSource.SetResult(null);
		}
		else
		{
			taskCompletionSource.SetException(task.Exception);
		}
	}
}