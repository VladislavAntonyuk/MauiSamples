namespace MauiBank.Views;

using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class CardPage : ContentPage
{
	public CardPage(CardPageViewModel cardPageViewModel)
	{
		InitializeComponent();
		BindingContext = cardPageViewModel;
	}
}

public partial class CardPageViewModel : ObservableObject
{
	[ObservableProperty]
	private string[] pages = { "1", "2", "3" };
	[ObservableProperty]
	private string currentPage = "2";
	[ObservableProperty]
	private List<Reward> rewards = new();
	[ObservableProperty]
	private List<TransactionGroup> transactions;

	public CardPageViewModel()
	{
		var transactionsList = new List<Transaction>();
		for (int i = 0; i < 60; i++)
		{
			rewards.Add(new Reward()
			{
				IsAchieved = Random.Shared.Next(0, 10) > 5,
				Image = "https://picsum.photos/50",
				Text = $"Reward {i}"
			});
			transactionsList.Add(new Transaction()
			{
				DateTime = new DateTime(DateTime.Now.Year - 1, Random.Shared.Next(1, 12), Random.Shared.Next(1, 28), Random.Shared.Next(0, 12), Random.Shared.Next(0, 59), Random.Shared.Next(0, 59)),
				Name = "Andrew",
				Description = "Send to card",
				Sum = $"{Random.Shared.Next(-1000, 1000) + Math.Round(Random.Shared.NextSingle(), 2)}$"
			});
		}

		transactions = transactionsList.OrderByDescending(x => x.DateTime)
									   .GroupBy(x => x.DateTime.Date)
									   .Select(tr => new TransactionGroup(tr.Key, tr.ToList()))
									   .ToList();
	}

	[RelayCommand]
	Task More()
	{
		return GetMainPage().GoToAsync("//home/profile", true);
	}

	AppShell GetMainPage()
	{
		ArgumentNullException.ThrowIfNull(Application.Current?.MainPage);
		return (AppShell)Application.Current.MainPage;
	}
}

public class Reward
{
	public bool IsAchieved { get; set; }
	public string? Image { get; set; }
	public string? Text { get; set; }
}
public class Transaction
{
	public DateTime DateTime { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Sum { get; set; }
}
public class TransactionGroup : List<Transaction>
{
	public DateOnly Date { get; }

	public TransactionGroup(DateTime dateTime, List<Transaction> transactions) : base(transactions)
	{
		Date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
	}
}

public class BoolToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return System.Convert.ToBoolean(value) ? Colors.LightGreen : Colors.LightGray;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

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