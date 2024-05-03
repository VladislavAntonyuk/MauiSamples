using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Handlers.Items;

namespace MauiStaggeredCollectionView;

public record Card(string Label, string Image);
public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		MyCollectionView.ItemsSource = new ObservableCollection<Card>()
		{
			new("Image 1","https://th.bing.com/th/id/OIP.wGvRsIjycKa3Asr03KO4KgHaDH?rs=1&pid=ImgDetMain"),
			new("Image 2", "https://th.bing.com/th/id/R.537268dc2d773fd8651824c36f1c78d3?rik=bCh1W7hrS5ljfQ&pid=ImgRaw&r=0"),
			new("Image 3", "https://th.bing.com/th/id/OIP.kWZbyYQqqFaowDIJVCoRSgHaEK?rs=1&pid=ImgDetMain"),
			new("Image 4", "https://learn.microsoft.com/en-us/dotnet/maui/media/what-is-maui/architecture-diagram.png"),
			new("Image 5", "https://th.bing.com/th/id/R.88d727f77173a5338fb70fed37b4131d?rik=tcl4LhWD6an5%2fg&pid=ImgRaw&r=0"),
			new("Image 6", "https://learn.microsoft.com/en-us/dotnet/maui/media/what-is-maui/maui.png"),
			new("Image 7", "https://th.bing.com/th/id/OIP.TTeAND36n5837q14OO5qqAHaGq?w=768&h=691&rs=1&pid=ImgDetMain"),
			new("Image 8", "https://th.bing.com/th/id/OIP.yvpmJbIaGIyatyAiTzbSTgHaHk?rs=1&pid=ImgDetMain"),
			new("Image 9", "https://th.bing.com/th/id/OIP.PsAQ5A0DMjL5866MIpa24QAAAA?w=383&h=811&rs=1&pid=ImgDetMain"),
			new("Image 10", "https://user-images.githubusercontent.com/271950/164788792-a96610d5-3910-45fe-b003-4db797eb2188.png"),
		};
	}
}

public class StaggeredStructuredItemsViewHandler : StructuredItemsViewHandler<CollectionView>
{
	public StaggeredStructuredItemsViewHandler() : base(StaggeredStructuredItemsViewMapper)
	{

	}
	public StaggeredStructuredItemsViewHandler(PropertyMapper? mapper = null) : base(mapper ?? StaggeredStructuredItemsViewMapper)
	{

	}

	public static PropertyMapper<CollectionView, StructuredItemsViewHandler<CollectionView>> StaggeredStructuredItemsViewMapper = new(StructuredItemsViewMapper)
	{
		[StructuredItemsView.ItemsLayoutProperty.PropertyName] = MapItemsLayout
	};

#if ANDROID
		private static void MapItemsLayout(StructuredItemsViewHandler<CollectionView> handler, CollectionView view)
		{
			var platformView = handler.PlatformView as MauiRecyclerView<CollectionView, ItemsViewAdapter<CollectionView, IItemsViewSource>, IItemsViewSource>;
			switch (view.ItemsLayout)
			{
				case StaggeredItemsLayout staggeredItemsLayout:
					platformView?.UpdateAdapter();
					platformView?.SetLayoutManager(
						new AndroidX.RecyclerView.Widget.StaggeredGridLayoutManager(
							staggeredItemsLayout.Span, 
							staggeredItemsLayout.Orientation == ItemsLayoutOrientation.Horizontal ? AndroidX.RecyclerView.Widget.StaggeredGridLayoutManager.Horizontal : AndroidX.RecyclerView.Widget.StaggeredGridLayoutManager.Vertical));
					break;
				default:
					platformView?.UpdateLayoutManager();
					break;
			}
		}
#endif

#if IOS || MACCATALYST
		protected override ItemsViewLayout SelectLayout()
		{
			var itemsLayout = ItemsView.ItemsLayout;

			if (itemsLayout is StaggeredItemsLayout staggeredItemsLayout)
			{
				return new StagLayout([(1d, 1d), (1d, 1d)], 0, staggeredItemsLayout, ItemSizingStrategy.MeasureAllItems);
			}

			return base.SelectLayout();
		}
#endif

#if WINDOWS
		protected override Microsoft.UI.Xaml.Controls.ListViewBase SelectListViewBase()
		{
			return this.VirtualView.ItemsLayout switch
			{
				StaggeredItemsLayout staggeredItemsLayout => new Microsoft.UI.Xaml.Controls.GridView() { },
				_ => base.SelectListViewBase()
			};
		}
#endif
}

public class StaggeredItemsLayout(ItemsLayoutOrientation orientation) : ItemsLayout(orientation)
{
	public static readonly BindableProperty SpanProperty = BindableProperty.Create(nameof(Span), typeof(int), typeof(StaggeredItemsLayout), default(int));
		
	public StaggeredItemsLayout() : this(ItemsLayoutOrientation.Vertical)
	{
			
	}

	public int Span
	{
		get => (int)GetValue(SpanProperty);
		set => SetValue(SpanProperty, value);
	}
}