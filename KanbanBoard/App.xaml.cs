namespace KanbanBoard;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;
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

	private async Task AddTestData()
	{
		using var scope = serviceProvider.CreateScope();
		await using var appContext = scope.ServiceProvider.GetRequiredService<KanbanBoardDbContext>();
		await appContext.Database.EnsureCreatedAsync();

		var items = await appContext.Columns.Include(x => x.Cards).ToArrayAsync();
		if (items.Length == 0)
		{
			var todoColumn = new Column
			{
				Name = "ToDo",
				Order = 0
			};
			var inProgressColumn = new Column
			{
				Name = "In Progress",
				Order = 1,
				Wip = 3
			};
			await appContext.AddAsync(todoColumn);
			await appContext.AddAsync(inProgressColumn);
			await appContext.AddAsync(new Column
			{
				Name = "Done",
				Order = 2
			});

			await appContext.AddAsync(new Card
			{
				Name = "Card 1",
				Description = "Description for card 1",
				Order = 0,
				Column = todoColumn
			});
			await appContext.AddAsync(new Card
			{
				Name = "Card 2",
				Description = "Description for card 2",
				Order = 1,
				Column = todoColumn
			});
			await appContext.AddAsync(new Card
			{
				Name = "Card 3",
				Description = "Description for card 3",
				Order = 0,
				Column = inProgressColumn
			});

			await appContext.SaveChangesAsync();
		}
	}
}