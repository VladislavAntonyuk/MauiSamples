#pragma warning disable CS0169
#pragma warning disable CS0414
namespace CardLayout;

using System.Collections;
using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

public partial class CardsLayout : Layout, ILayoutManager
{
	[AutoBindable(DefaultValue = "5")]
	private int spacing = 5;

	[AutoBindable(DefaultValue = "0.8")]
	private double cardScaling = 0.8;

	[AutoBindable(OnChanged = "ItemsChanged", DefaultBindingMode = nameof(BindingMode.OneWay))]
	private ICollection? items;

	[AutoBindable]
	private DataTemplate? itemTemplate;

	[AutoBindable]
	private IView? emptyView;

	[AutoBindable]
	private CardLayoutDirection direction;

	[AutoBindable]
	private SwipeDirection swipeDirection;

	private readonly Stack<IView> cards = new();

	private void ItemsChanged()
	{
		Children.Clear();
		if (Items is null)
		{
			return;
		}

		foreach (var item in Items)
		{
			Children.Add(ViewFor(item));
		}
	}

	protected override void OnChildAdded(Element child)
	{
		if (Children.Count > 1 && EmptyView is not null && Children[0] == EmptyView)
		{
			Children.Remove(EmptyView);
		}
		else
		{
			base.OnChildAdded(child);
		}
	}

	protected override void OnChildRemoved(Element child, int oldLogicalIndex)
	{
		if (Children.Count == 0 && EmptyView is not null)
		{
			Children.Add(EmptyView);
		}

		base.OnChildRemoved(child, oldLogicalIndex);
	}

	protected virtual View? ViewFor(object item)
	{
		View? view = null;

		if (ItemTemplate != null)
		{
			view = (View)ItemTemplate.CreateContent();
			view.BindingContext = item;
		}

		return view;
	}

	public CardsLayout()
	{
		var panGesture = new PanGestureRecognizer();
		panGesture.PanUpdated += PanGesture_PanUpdated;
		GestureRecognizers.Add(panGesture);
		//var swipeGesture = new SwipeGestureRecognizer();
		//swipeGesture.Swiped += SwipeGesture_Swiped;
		//GestureRecognizers.Add(swipeGesture);
	}

	private void SwipeGesture_Swiped(object? sender, SwipedEventArgs e)
	{
		HandleTouchEnd(e.Direction);
	}

	private SwipeDirection? swipedDirection;
	private void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
	{
		switch (e.StatusType)
		{
			case GestureStatus.Running:
				HandleTouch(e.TotalX, e.TotalY);
				break;
			case GestureStatus.Completed:
				HandleTouchEnd(swipedDirection);
				break;
		}
	}

	private void HandleTouch(double eTotalX, double eTotalY)
	{
		swipedDirection = null;
		const int delta = 50;
		swipedDirection = eTotalX switch
		{
			> delta => SwipeDirection.Right,
			< -delta => SwipeDirection.Left,
			_ => eTotalY switch
			{
				> delta => SwipeDirection.Down,
				< -delta => SwipeDirection.Up,
				_ => swipedDirection
			}
		};
	}

	private void HandleTouchEnd(SwipeDirection? swiped)
	{
		if (swiped == null)
		{
			return;
		}

		switch (SwipeDirection)
		{
			case SwipeDirection.Right:
				switch (swiped)
				{
					case SwipeDirection.Right when Children.Count > 0:
						cards.Push(Children[^1]);
						Children.RemoveAt(Children.Count - 1);
						break;
					case SwipeDirection.Left when cards.Count > 0:
						Children.Add(cards.Pop());
						break;
				}
				break;
			case SwipeDirection.Left:
				switch (swiped)
				{
					case SwipeDirection.Left when Children.Count > 0:
						cards.Push(Children[^1]);
						Children.RemoveAt(Children.Count - 1);
						break;
					case SwipeDirection.Right when cards.Count > 0:
						Children.Add(cards.Pop());
						break;
				}
				break;
			case SwipeDirection.Up:
				switch (swiped)
				{
					case SwipeDirection.Up when Children.Count > 0:
						cards.Push(Children[^1]);
						Children.RemoveAt(Children.Count - 1);
						break;
					case SwipeDirection.Down when cards.Count > 0:
						Children.Add(cards.Pop());
						break;
				}
				break;
			case SwipeDirection.Down:
				switch (swiped)
				{
					case SwipeDirection.Down when Children.Count > 0:
						cards.Push(Children[^1]);
						Children.RemoveAt(Children.Count - 1);
						break;
					case SwipeDirection.Up when cards.Count > 0:
						Children.Add(cards.Pop());
						break;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

	}

	public Size ArrangeChildren(Rect rectangle)
	{
		if (Children.Count == 0)
		{
			if (EmptyView is null)
			{
				return new Size(Padding.HorizontalThickness, Padding.VerticalThickness);
			}

			var emptyViewSize = EmptyView.Arrange(rectangle);
			return new Size(emptyViewSize.Width + Padding.HorizontalThickness,
							emptyViewSize.Height + Padding.VerticalThickness);
		}

		int i = Children.Count - 1;
		double x = Padding.Left;
		double y = Direction == CardLayoutDirection.DownToUp ? Spacing * i + Padding.Top : Padding.Top;

		double totalWidth = 0;
		double totalHeight = 0;
		var maxWidth = Children[^1].DesiredSize.Width;
		var maxHeight = Children[^1].DesiredSize.Height;
		foreach (var child in Children)
		{
			double width;
			double height;
			switch (Direction)
			{
				case CardLayoutDirection.LeftToRight:
					width = child.DesiredSize.Width;
					height = child.DesiredSize.Height * Math.Pow(CardScaling, i);
					child.Arrange(new Rect(x,
										   y + (maxHeight - height) / 2,
										   width,
										   height));
					x += Spacing;
					break;
				case CardLayoutDirection.RightToLeft:
					width = child.DesiredSize.Width;
					height = child.DesiredSize.Height * Math.Pow(CardScaling, i);
					child.Arrange(new Rect(x,
										   y + (maxHeight - height) / 2,
										   width,
										   height));
					break;
				case CardLayoutDirection.UpToDown:
					width = child.DesiredSize.Width * Math.Pow(CardScaling, i);
					height = child.DesiredSize.Height;
					child.Arrange(new Rect(x + (maxWidth - width) / 2,
										   y,
										   width,
										   height));
					y += Spacing;
					break;
				case CardLayoutDirection.DownToUp:
					width = child.DesiredSize.Width * Math.Pow(CardScaling, i);
					height = child.DesiredSize.Height;
					child.Arrange(new Rect(x + (maxWidth - width) / 2,
										   y,
										   width,
										   height));
					y -= Spacing;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			totalWidth = Math.Max(totalWidth, x + width);
			totalHeight = Math.Max(totalHeight, y + height);
			i--;
		}

		return new Size(totalWidth + Padding.HorizontalThickness,
						totalHeight + Padding.VerticalThickness);
	}

	public Size Measure(double widthConstraint, double heightConstraint)
	{
		widthConstraint -= Padding.HorizontalThickness;
		heightConstraint -= Padding.VerticalThickness;

		if (Children.Count == 0)
		{
			if (EmptyView is null)
			{
				return new Size(Padding.HorizontalThickness, Padding.VerticalThickness);
			}

			var emptyViewSize = EmptyView.Measure(widthConstraint, heightConstraint);
			return new Size(emptyViewSize.Width + Padding.HorizontalThickness,
							emptyViewSize.Height + Padding.VerticalThickness);
		}


		double x = Padding.Left;
		double y = Padding.Top;
		double totalWidth = 0;
		double totalHeight = 0;

		foreach (var child in Children)
		{
			switch (Direction)
			{
				case CardLayoutDirection.LeftToRight:
					x += Spacing;
					widthConstraint -= Spacing;
					break;
				case CardLayoutDirection.RightToLeft:
					widthConstraint -= Spacing;
					break;
				case CardLayoutDirection.UpToDown:
					y += Spacing;
					heightConstraint -= Spacing;
					break;
				case CardLayoutDirection.DownToUp:
					heightConstraint += Spacing;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			var current = child.Measure(widthConstraint, heightConstraint);
			totalWidth = Math.Max(totalWidth, x + current.Width);
			totalHeight = Math.Max(totalHeight, y + current.Height);
		}

		return new Size(totalWidth + Padding.HorizontalThickness,
						totalHeight + Padding.VerticalThickness);
	}

	protected override ILayoutManager CreateLayoutManager() => this;
}