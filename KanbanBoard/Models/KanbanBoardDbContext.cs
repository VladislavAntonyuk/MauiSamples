namespace KanbanBoard.Models;

using Microsoft.EntityFrameworkCore;

public class KanbanBoardDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseModel(KanbanBoardDbContextModel.Instance);
		optionsBuilder.UseSqlite($"Filename={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KanbanBoard.db")}");
	}

	public DbSet<Card> Cards => Set<Card>();
	public DbSet<Column> Columns => Set<Column>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<Column>().HasMany(x => x.Cards).WithOne(x => x.Column);
	}
}