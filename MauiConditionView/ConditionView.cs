namespace MauiConditionView;

public class ConditionView : ContentView
{
	public static readonly BindableProperty FalseProperty = BindableProperty.Create(nameof(False), typeof(View), typeof(ConditionView));
	public static readonly BindableProperty TrueProperty = BindableProperty.Create(nameof(True), typeof(View), typeof(ConditionView));
	public static readonly BindableProperty IfProperty = BindableProperty.Create(nameof(If), typeof(bool), typeof(ConditionView), false, propertyChanged: ConditionChanged);

	private static void ConditionChanged(BindableObject bindable, object oldvalue, object newvalue)
	{
		var conditionView = (ConditionView)bindable;
		conditionView.Content = (bool)newvalue ? conditionView.True : conditionView.False;
	}

	public bool If
	{
		get => (bool)GetValue(IfProperty);
		set => SetValue(IfProperty, value);
	}

	public View True
	{
		get => (View)GetValue(TrueProperty);
		set => SetValue(TrueProperty, value);
	}

	public View False
	{
		get => (View)GetValue(FalseProperty);
		set => SetValue(FalseProperty, value);
	}
}