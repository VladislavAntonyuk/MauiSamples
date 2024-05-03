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
			new("Image 1","https://picsum.photos/200/200"),
			new("Image 2", "https://picsum.photos/100/50"),
			new("Image 3", "https://picsum.photos/100/200"),
			new("Image 4", "https://picsum.photos/200/100"),
			new("Image 5", "https://picsum.photos/150/100"),
			new("Image 6", "https://picsum.photos/120/150"),
			new("Image 7", "https://picsum.photos/180/140"),
			new("Image 8", "https://picsum.photos/200/50"),
			new("Image 9", "https://picsum.photos/50/150"),
			new("Image 10", "https://picsum.photos/50/100"),
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