namespace MauiConditionView;

public class SwitchCaseView<T> : ContentView
	where T : notnull
{
	public static readonly BindableProperty ConditionsProperty = BindableProperty.Create(nameof(Conditions), typeof(ICollection<CaseView<T>>), typeof(SwitchCaseView<T>), new List<CaseView<T>>(), propertyChanged: SwitchChanged);
	public static readonly BindableProperty DefaultProperty = BindableProperty.Create(nameof(Default), typeof(View), typeof(SwitchCaseView<T>), propertyChanged: SwitchChanged);
	public static readonly BindableProperty SwitchProperty = BindableProperty.Create(nameof(Switch), typeof(T), typeof(SwitchCaseView<T>), propertyChanged: SwitchChanged);

	private static void SwitchChanged(BindableObject bindable, object oldvalue, object newvalue)
	{
		var switchCaseView = (SwitchCaseView<T>)bindable;
		switchCaseView.Content = switchCaseView.Conditions
		                                     .Where(x => x.Case.Equals(switchCaseView.Switch))
		                                     .Select(x => x.Content)
		                                     .SingleOrDefault(switchCaseView.Default);
	}

	public T Switch
	{
		get => (T)GetValue(SwitchProperty);
		set => SetValue(SwitchProperty, value);
	}

	public View? Default
	{
		get => (View?)GetValue(DefaultProperty);
		set => SetValue(DefaultProperty, value);
	}

	public ICollection<CaseView<T>> Conditions
	{
		get => (ICollection<CaseView<T>>)GetValue(ConditionsProperty);
		set => SetValue(ConditionsProperty, value);
	}
}

public class CaseView<T> : ContentView
{
	public static readonly BindableProperty CaseProperty = BindableProperty.Create(nameof(Case), typeof(T), typeof(CaseView<T>));
	public T Case
	{
		get => (T)GetValue(CaseProperty);
		set => SetValue(CaseProperty, value);
	}
}