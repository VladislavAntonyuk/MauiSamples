namespace MauiSqlite;
using SqliteRepository;

public partial class MainPage : ContentPage
{
	private readonly AccountRepository _accountRepository;

	public MainPage(AccountRepository accountRepository)
	{
		_accountRepository = accountRepository;
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		GetAccounts();
	}

	private void AddAccountClicked(object sender, EventArgs e)
	{
		var account = new Account()
		{
			Balance = Random.Shared.Next(0, 10),
			Email = "test@email.com"
		};
		_accountRepository.CreateAccount(account);
		GetAccounts();
	}

	private void UpdateAccountClicked(object sender, EventArgs e)
	{
		if (collectionView.SelectedItem is null)
			return;

		var account = collectionView.SelectedItem as Account;
		if (account is null)
			return;

		account.Balance = 0;
		_accountRepository.UpdateAccount(account);
		GetAccounts();
	}

	private void DeleteAccountClicked(object sender, EventArgs e)
	{
		if (collectionView.SelectedItem is null)
			return;

		var account = collectionView.SelectedItem as Account;
		if (account is null)
			return;

		_accountRepository.DeleteAccount(account);
		GetAccounts();
	}

	private void Filter1AccountClicked(object sender, EventArgs e)
	{
		collectionView.ItemsSource = _accountRepository.QueryAccountWithPositiveBalance();
	}

	private void Filter2AccountClicked(object sender, EventArgs e)
	{
		collectionView.ItemsSource = _accountRepository.LinqZeroBalance();
	}

	private void GetAccounts()
	{
		collectionView.ItemsSource = _accountRepository.GetAccounts();
	}
}