namespace MauiStaggeredCollectionView;

using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls.Handlers.Items;
using UIKit;

public class StaggeredItemsViewLayout(StaggeredItemsLayout itemsLayout, ItemSizingStrategy sizingStrategy)
	: ItemsViewLayout(itemsLayout, sizingStrategy)
{
	private readonly List<UICollectionViewLayoutAttributes> cache = new();

	private float contentHeight;

	public int ColumnCount { get; set; } = itemsLayout.Span;

	public double MinCellHeight { get; set; } = 150;

	public double MaxCellHeight { get; set; } = 250;

	public override CGSize CollectionViewContentSize => new(CollectionView.Frame.Width, contentHeight);

	public override void ConstrainTo(CGSize size)
	{
		ConstrainedDimension = ScrollDirection == UICollectionViewScrollDirection.Vertical ? size.Width : size.Height;
		DetermineCellSize();
	}

	protected void OnItemsSourcePropertyChanged()
	{
		cache.Clear();
	}

	public override void PrepareLayout()
	{
		if (cache.Count > 0)
		{
			return;
		}

		contentHeight = 0;
		var columnWidth = (float)CollectionView.Frame.Width / ColumnCount;
		var xOffsets = new List<float>();
		var yOffsets = new List<float>();
		for (var i = 0; i < ColumnCount; i++)
		{
			xOffsets.Add(i * columnWidth);
			yOffsets.Add(0);
		}

		var column = 0;

		var itemsCount = CollectionView.NumberOfItemsInSection(0).ToInt32();
		for (var i = 0; i < itemsCount; i++)
		{
			var indexPath = NSIndexPath.FromRowSection(i, 0);

			var cellHeight = new Random().Next((int)MinCellHeight, (int)MaxCellHeight);

			var frame = new CGRect(xOffsets[column], yOffsets[column], columnWidth, cellHeight);

			var attrs = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
			attrs.Frame = frame;
			cache.Add(attrs);

			contentHeight = Math.Max(contentHeight, (float)frame.GetMaxY());
			yOffsets[column] += cellHeight;

			column = column == ColumnCount - 1 ? 0 : column + 1;
		}
	}

	public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
	{
		var visibleLayoutAttributes = new List<UICollectionViewLayoutAttributes>();

		foreach (var attr in cache)
		{
			if (attr.Frame.IntersectsWith(rect))
			{
				visibleLayoutAttributes.Add(attr);
			}
		}

		return visibleLayoutAttributes.ToArray();
	}

	public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
	{
		return cache[indexPath.Row];
	}
}