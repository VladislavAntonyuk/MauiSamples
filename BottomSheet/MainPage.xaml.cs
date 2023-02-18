namespace BottomSheet;

#if ANDROID
using BottomSheetView =  Google.Android.Material.BottomSheet.BottomSheetDialog;
#elif IOS || MACCATALYST
using BottomSheetView = UIKit.UIViewController;
#elif TIZEN
using BottomSheetView = Tizen.UIExtensions.NUI.Popup;
#else
using BottomSheetView = Microsoft.UI.Xaml.Controls.Primitives.Popup;
#endif

public partial class MainPage : ContentPage, IDisposable
{
	BottomSheetView? bottomSheet;
	public int Count { get; set; }
	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		Count++;
		OnPropertyChanged(nameof(Count));
	}

	private void ShowBottomSheet(object sender, EventArgs e)
	{
		bottomSheet = this.ShowBottomSheet(GetBottomSheetView(), true);
	}

	private View GetBottomSheetView()
	{
		var view = (View)BottomSheetTemplate.CreateContent();
		view.BindingContext = BindingContext;
		return view;
	}

	private void ShowBottomSheetWithLongContent(object sender, EventArgs e)
	{
		bottomSheet = this.ShowBottomSheet(GetBottomSheetViewWithLongContent(), false);
	}

	private View GetBottomSheetViewWithLongContent()
	{
		var view = (View)BottomSheetTemplateWithLongContent.CreateContent();
		view.BindingContext = BindingContext;
		return view;
	}

	private void OnCloseClicked(object? sender, EventArgs e)
	{
		bottomSheet?.CloseBottomSheet();
	}

	public void Dispose()
	{
#if !WINDOWS
		bottomSheet?.Dispose();
#endif
	}
}