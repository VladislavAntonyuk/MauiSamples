namespace CardLayout;

using System.Collections;
using Maui.BindableProperty.Generator.Core;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Graphics;

public partial class CardLayout : Layout, ILayoutManager
{
#pragma warning disable CS0414
	[AutoBindable(DefaultValue = "5")]
	private int spacing = 5;

	[AutoBindable(DefaultValue = "0.8")]
	private double cardScaling = 0.8;

#pragma warning disable CS0169
	[AutoBindable(OnChanged = "ItemsChanged", DefaultBindingMode = nameof(BindingMode.OneWay))]
	private ICollection items;

	[AutoBindable]
	private DataTemplate itemTemplate;
#pragma warning restore CS0169
#pragma warning restore CS0414

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

	protected virtual View ViewFor(object item)
	{
		View view = null;

		if (ItemTemplate != null)
		{
			view = (View)ItemTemplate.CreateContent();
			view.BindingContext = item;
		}

		return view;
	}

	private readonly Stack<IView> cards = new();

	public CardLayout()
	{
		var swipeGesture = new SwipeGestureRecognizer();
		swipeGesture.Direction = SwipeDirection.Right | SwipeDirection.Left;
		swipeGesture.Swiped += SwipeGesture_Swiped;
		GestureRecognizers.Add(swipeGesture);
		var panGesture = new PanGestureRecognizer();
		panGesture.PanUpdated += PanGesture_PanUpdated; ;
		GestureRecognizers.Add(panGesture);
	}

	private SwipeDirection direction;
	private void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
	{
		switch (e.StatusType)
		{
			case GestureStatus.Started:
				HandleTouchStart();
				break;
			case GestureStatus.Running:
				HandleTouch(e.TotalX);
				break;
			case GestureStatus.Completed:
				HandleTouchEnd(direction);
				break;
			default:
				break;
		}
	}

	private void HandleTouchStart()
	{
	}

	private void HandleTouch(double eTotalX)
	{
		direction = eTotalX > 0 ? SwipeDirection.Right : SwipeDirection.Left;
	}

	private void HandleTouchEnd(SwipeDirection sweepDirection)
	{
		switch (sweepDirection)
		{
			case SwipeDirection.Right when Children.Count > 0:
				cards.Push(Children[^1]);
				Children.RemoveAt(Children.Count - 1);
				break;
			case SwipeDirection.Left when cards.Count > 0:
				Children.Add(cards.Pop());
				break;
		}
	}

	private void SwipeGesture_Swiped(object sender, SwipedEventArgs e)
	{
		HandleTouchEnd(e.Direction);
	}

	public Size ArrangeChildren(Rect rectangle)
	{
		double x = Padding.Left;
		double y = Padding.Top;

		double totalWidth = 0;
		double totalHeight = 0;

		int i = Children.Count - 1;
		var maxHeight = Children[^1].DesiredSize.Height;
		foreach (var child in Children)
		{
			var width = child.DesiredSize.Width;
			var height = child.DesiredSize.Height * Math.Pow(CardScaling, i);
			child.Arrange(new Rect(x,
								   y + (maxHeight - height) / 2,
								   width,
								   height));

			totalWidth = Math.Max(totalWidth, x + width);
			totalHeight = Math.Max(totalHeight, y + height);

			x += Spacing;
			i--;
		}

		return new Size(totalWidth + Padding.HorizontalThickness,
						totalHeight + Padding.VerticalThickness);
	}

	public Size Measure(double widthConstraint, double heightConstraint)
	{
		widthConstraint -= Padding.HorizontalThickness;
		heightConstraint -= Padding.VerticalThickness;

		double x = Padding.Left;
		double y = Padding.Top;
		double totalWidth = 0;
		double totalHeight = 0;

		foreach (var child in Children)
		{
			var current = child.Measure(widthConstraint, heightConstraint);
			totalWidth = Math.Max(totalWidth, x + current.Width);
			totalHeight = Math.Max(totalHeight, y + current.Height);

			x += Spacing;

			widthConstraint -= Spacing;
		}

		return new Size(totalWidth + Padding.HorizontalThickness,
						totalHeight + Padding.VerticalThickness);
	}

	protected override ILayoutManager CreateLayoutManager() => this;
}