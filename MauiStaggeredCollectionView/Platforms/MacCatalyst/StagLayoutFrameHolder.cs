using CoreGraphics;
using Foundation;
using UIKit;

namespace MauiStaggeredCollectionView;

public class StagLayoutFrameHolder
{
	private List<UICollectionViewLayoutAttributes> cache = new List<UICollectionViewLayoutAttributes>();

	private readonly List<(double, double)> widthHeightRatios;
	private double contentWidth;
	private double contentHeight;
	private readonly double itemSpacing;

	public bool Prepared => cache.Any();

	public CGSize CollectionViewContentSize => new CGSize(contentWidth, contentHeight);

	public StagLayoutFrameHolder(List<(double, double)> widthHeightRatios, double itemSpacing)
	{
		this.widthHeightRatios = widthHeightRatios;
		this.itemSpacing = itemSpacing;
	}

	public void Prepare(int itemCount, double contentWidth)
	{
		this.contentWidth = contentWidth;

		double xOffset = 0;
		double yOffset = 0;
		CGRect? previousItemFrame = null;
		int ratioIndex = 0;

		for (int index = 0; index < itemCount; index++)
		{
			if (ratioIndex >= widthHeightRatios.Count)
			{
				ratioIndex = 0;
			}

			var ratios = widthHeightRatios[ratioIndex];
			if (ratios.Item1 < 0.0f || ratios.Item1 > 1.0f || ratios.Item2 < 0.0f)
			{
				throw new ArgumentOutOfRangeException("Width ratio should be in [0.0, 1.0], with height ratio being in [0.0, ∞)");
			}

			bool isFullWidth = ratios.Item1 == 1.0d;
			double halfItemSpacing = itemSpacing * 0.5f;
			(double width, double height) = (
				(contentWidth * ratios.Item1) - (isFullWidth ? 0 : halfItemSpacing),
				contentWidth * ratios.Item2
			);

			CGRect frame = new CGRect(xOffset, yOffset, width, height);

			var indexPath = NSIndexPath.Create(index, 0);
			var attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
			attributes.Frame = frame;
			cache.Add(attributes);

			contentHeight = Math.Max(contentHeight, frame.Bottom);

			bool hasReachedEnd = xOffset + width >= contentWidth;
			bool isItemNextToIt;
			if (previousItemFrame.HasValue)
			{
				// check whether there's still room to place another item to the right of it
				bool isThereSpaceNextToIt = frame.Bottom <= previousItemFrame.Value.Bottom;
				// Make sure there's no item next to it
				isItemNextToIt = !isFullWidth && (frame.Bottom - itemSpacing) <= previousItemFrame.Value.Bottom;

				// If there's an item next to it and no space, the next item should have an xOffset of 0
				// and height will auto correct to not include spacing so that it is perfectly aligned
				if (!isThereSpaceNextToIt && isItemNextToIt)
				{
					frame = new CGRect(xOffset, yOffset, width, height - itemSpacing);
					attributes.Frame = frame;
					cache.RemoveAt(cache.Count - 1);
					cache.Add(attributes);
				}
			}
			else
			{
				isItemNextToIt = false;
			}

			if (isFullWidth || hasReachedEnd || isItemNextToIt)
			{
				xOffset = 0;
			}
			else
			{
				xOffset += frame.Width + itemSpacing;
			}

			if (isFullWidth)
			{
				yOffset = frame.Bottom + itemSpacing;
			}
			else
			{
				if (previousItemFrame.HasValue)
				{
					yOffset = Math.Min(frame.Bottom.Value, previousItemFrame.Value.Bottom.Value) + itemSpacing;
				}
			}

			previousItemFrame = frame;
			ratioIndex++;
		}
	}

	public UICollectionViewLayoutAttributes[] LayoutAttributesForElements(CGRect rect)
	{
		return cache.Where(attributes => attributes.Frame.IntersectsWith(rect)).ToArray();
	}

	public UICollectionViewLayoutAttributes? LayoutAttributesForItem(NSIndexPath indexPath)
	{
		return cache.ElementAtOrDefault(indexPath.Item.ToInt32());
	}

	public void InvalidateLayout()
	{
		cache.Clear();
	}
}