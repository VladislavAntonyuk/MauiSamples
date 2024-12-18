namespace MauiBank.ViewModels;

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

public partial class CardPageViewModel : ObservableObject
{
	[ObservableProperty]
	public partial string[] Pages { get; set; }
	[ObservableProperty]
	public partial string CurrentPage { get; set; }
	[ObservableProperty]
	public partial List<Reward> Rewards { get; set; }
	[ObservableProperty]
	public partial List<TransactionGroup> Transactions { get; set; }
	[ObservableProperty]
	public partial bool IsCvvMode { get; set; }

	public CardPageViewModel()
	{
		Pages = ["1", "2", "3"];
		CurrentPage = "2";
		Rewards = new();
		var transactionsList = new List<Transaction>();
		for (int i = 0; i < 60; i++)
		{
			Rewards.Add(new Reward()
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

		Transactions = transactionsList.OrderByDescending(x => x.DateTime)
									   .GroupBy(x => x.DateTime.Date)
									   .Select(tr => new TransactionGroup(tr.Key, tr.ToList()))
									   .ToList();
	}

	public ICommand SwitchCvvModeCommand => new Command(() => IsCvvMode = !IsCvvMode);

	[RelayCommand]
	Task More()
	{
		var page = Application.Current?.Windows.LastOrDefault()?.Page as AppShell;
		if (page is null)
		{
			return Task.CompletedTask;
		}

		return page.GoToAsync("//home/profile", true);
	}
}