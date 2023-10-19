using Microsoft.EntityFrameworkCore;

using task_list_app_maui.Models;

namespace task_list_app_maui.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tarefa> Tarefas { get; set; }
}
