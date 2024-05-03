using CoreGraphics;
using Foundation;
using System.Linq;
using Microsoft.Maui.Controls.Handlers.Items;
using UIKit;

namespace MauiStaggeredCollectionView;

public class StagLayout : ItemsViewLayout
{
	private float contentWidth;

	private StagLayoutFrameHolder frameHolder;

	public StagLayout(List<(double, double)> widthHeightRatios, float itemSpacing, ItemsLayout itemsLayout, ItemSizingStrategy sizingStrategy):base(itemsLayout, sizingStrategy)
	{
		frameHolder = new StagLayoutFrameHolder(widthHeightRatios, itemSpacing);
	}

	public override CGSize CollectionViewContentSize => frameHolder.CollectionViewContentSize;

	public override void ConstrainTo(CGSize size)
	{
		
	}

	public override void PrepareLayout()
	{
		base.PrepareLayout();

		if (!frameHolder.Prepared || CollectionView.NumberOfSections() == 0)
		{
			return;
		}
		frameHolder.Prepare(CollectionView.NumberOfItemsInSection(0).ToInt32(), contentWidth);
	}

		
	public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
	{
		return frameHolder.LayoutAttributesForElements(rect);
	}

	public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
	{
		return frameHolder.LayoutAttributesForItem(indexPath);
	}

	public override void InvalidateLayout()
	{
		base.InvalidateLayout();
		frameHolder.InvalidateLayout();
	}
}