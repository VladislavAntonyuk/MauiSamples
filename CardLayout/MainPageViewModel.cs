namespace CardLayout;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class MainPageViewModel : ObservableObject
{
	public ObservableCollection<Pizza> Pizzas =>
	[
		new()
		{
			Name = "Margherita",
			Description = "Margherita pizza is a classic Italian pizza topped with fresh mozzarella cheese, tomatoes, and basil leaves.",
			Price = 5.99m,
			Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/grumbling/pizza-hotone-pan.png"
		},
		new()
		{
			Name = "Pepperoni",
			Description = "Pepperoni pizza is an American pizza variety1. It is typically topped with pepperoni, a type of cured sausage made with a mixture of beef, pork, and spices.",
			Price = 6.99m,
			Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-halaal-hawaiian-thin.png"
		},
		new()
		{
			Name = "Hawaiian",
			Description = "Hawaiian pizza is a type of pizza that typically features tomato sauce, cheese, ham or Canadian bacon, and pineapple as the main toppings.",
			Price = 7.99m,
			Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-hawaiian-pan.png"
		},
		new()
		{
			Name = "Vegetarian",
			Description = "Vegetarian pizza is a delightful fusion of dough, sauce, and toppings that are free from meat, embracing a bounty of vegetables, cheeses, and herbs. It’s a canvas of culinary creativity, catering to those seeking a meatless option without sacrificing flavor.",
			Price = 8.99m,
			Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-vegetarian-pan.png"
		}
	];
}