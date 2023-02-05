namespace CardLayout;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class MainPageViewModel : ObservableObject
{

	[ObservableProperty]
	private ObservableCollection<Pizza> pizzas;

	public MainPageViewModel()
	{
		pizzas = new()
		{
			new Pizza
			{
				Name = "Margherita",
				Price = 5.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/grumbling/pizza-hotone-pan.png"
			},
			new Pizza
			{
				Name = "Pepperoni",
				Price = 6.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-halaal-hawaiian-thin.png"
			},
			new Pizza
			{
				Name = "Hawaiian",
				Price = 7.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-hawaiian-pan.png"
			},
			new Pizza
			{
				Name = "Vegetarian",
				Price = 8.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-vegetarian-pan.png"
			},
			new Pizza
			{
				Name = "Meat Lovers",
				Price = 9.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/grumbling/pizza-bolognaise-pan.png"
			},
			new Pizza
			{
				Name = "Supreme",
				Price = 10.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-regina-pan.png"
			}
		};
	}
}