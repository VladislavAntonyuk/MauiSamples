namespace MauiConditionView;

using System.Globalization;

public partial class NumericUpDown
{
	public NumericUpDown()
	{
		InitializeComponent();
		ValueControl.Text = Value.ToString(CultureInfo.CurrentCulture);
	}

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NumericUpDown), 0.0, BindingMode.TwoWay);
	public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(double), typeof(NumericUpDown), 0.0);
	public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(double), typeof(NumericUpDown), 100.0);
	public static readonly BindableProperty IncrementProperty = BindableProperty.Create(nameof(Increment), typeof(double), typeof(NumericUpDown), 1.0);

	public double Value
	{
		get => (double)GetValue(ValueProperty);
		set
		{
			var newValue = Math.Clamp(value, Minimum, Maximum);
			if (!newValue.Equals((double)GetValue(ValueProperty)))
			{
				SetValue(ValueProperty, newValue);
				ValueControl.Text = newValue.ToString(CultureInfo.CurrentCulture);
			}
		}
	}

	public double Minimum
	{
		get => (double)GetValue(MinimumProperty);
		set => SetValue(MinimumProperty, value);
	}

	public double Maximum
	{
		get => (double)GetValue(MaximumProperty);
		set => SetValue(MaximumProperty, value);
	}

	public double Increment
	{
		get => (double)GetValue(IncrementProperty);
		set => SetValue(IncrementProperty, value);
	}

	private void IncrementClicked(object sender, EventArgs e)
	{
		Value += Increment;
	}

	private void DecrementClicked(object sender, EventArgs e)
	{
		Value -= Increment;
	}
}