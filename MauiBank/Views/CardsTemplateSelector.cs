namespace MauiBank.Views;

public class CardsTemplateSelector : DataTemplateSelector
{
	public DataTemplate? Rewards { get; set; }
	public DataTemplate? Main { get; set; }
	public DataTemplate? Card { get; set; }

	protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
	{
		return item switch
		{
			"1" => Rewards,
			"3" => Card,
			_ => Main,
		} ?? throw new NullReferenceException();
	}
}