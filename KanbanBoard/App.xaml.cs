namespace KanbanBoard;
using KanbanBoard.Db;
using KanbanBoard.Models;
using Application = Microsoft.Maui.Controls.Application;

public partial class App : Application
{
	private readonly IServiceProvider serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		this.serviceProvider = serviceProvider;
		AddTestData().Wait();
		MainPage = serviceProvider.GetRequiredService<MainPage>();
	}

	public async Task AddTestData()
	{
		using (var scope = serviceProvider.CreateScope())
		{
			await using var appContext = scope.ServiceProvider.GetRequiredService<KanbanBoardDbContext>();
			await appContext.Database.EnsureCreatedAsync();
		}

		var columnsRepository = serviceProvider.GetRequiredService<IColumnsRepository>();
		var cardsRepository = serviceProvider.GetRequiredService<ICardsRepository>();
		var items = await cardsRepository.GetItems();
		if (!items.Any())
		{
			var todoColumn = new Column { Name = "ToDo", Order = 0 };
			var inProgressColumn = new Column { Name = "In Progress", Order = 1, Wip = 3 };
			await columnsRepository.SaveItem(todoColumn);
			await columnsRepository.SaveItem(inProgressColumn);
			await columnsRepository.SaveItem(new Column { Name = "Done", Order = 2 });

			await cardsRepository.SaveItem(new Card { Name = "Card 1", Description = "Description for card 1", Order = 0, ColumnId = todoColumn.Id });
			await cardsRepository.SaveItem(new Card { Name = "Card 2", Description = "Description for card 2", Order = 1, ColumnId = todoColumn.Id });
			await cardsRepository.SaveItem(new Card
			{
				Name = "Card 3",
				Description = "Description for card 3",
				Order = 0,
				ColumnId = inProgressColumn.Id
			});
		}
	}
}