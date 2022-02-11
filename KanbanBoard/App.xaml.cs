﻿using KanbanBoard.Db;
using KanbanBoard.Models;
using Microsoft.EntityFrameworkCore;
using Application = Microsoft.Maui.Controls.Application;

namespace KanbanBoard;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        AddTestData().Wait();
        MainPage = serviceProvider.GetRequiredService<MainPage>();
    }

    public async Task AddTestData()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            await using var appContext = scope.ServiceProvider.GetRequiredService<KanbanBoardDbContext>();
            await appContext.Database.EnsureCreatedAsync();
        }

        var columnsRepository = _serviceProvider.GetRequiredService<IColumnsRepository>();
        var cardsRepository = _serviceProvider.GetRequiredService<ICardsRepository>();
        var items = await cardsRepository.GetItems();
        if (!items.Any())
        {
            var todoColumn = new Column { Name = "ToDo", Order = 1 };
            var inProgressColumn = new Column { Name = "In Progress", Order = 2, Wip = 3 };
            await columnsRepository.SaveItem(todoColumn);
            await columnsRepository.SaveItem(inProgressColumn);
            await columnsRepository.SaveItem(new Column { Name = "Done", Order = 3 });

            await cardsRepository.SaveItem(new Card { Name = "Card 1", Description = "Description for card 1", Order = 1, ColumnId = todoColumn.Id });
            await cardsRepository.SaveItem(new Card { Name = "Card 2", Description = "Description for card 2", Order = 2, ColumnId = todoColumn.Id });
            await cardsRepository.SaveItem(new Card
            {
                Name = "Card 3",
                Description = "Description for card 3",
                Order = 1,
                ColumnId = inProgressColumn.Id
            });
        }
    }
}
