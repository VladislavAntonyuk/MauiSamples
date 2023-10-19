using Microsoft.EntityFrameworkCore;

using task_list_app_maui.Models;

namespace task_list_app_maui.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Tarefa> Tarefas { get; set; }
}
