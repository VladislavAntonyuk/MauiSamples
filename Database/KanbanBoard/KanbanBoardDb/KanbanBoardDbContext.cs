namespace KanbanBoardDb;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class KanbanBoardDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite($"Filename={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KanbanBoard.db")}")
		              .LogTo(_ => Console.WriteLine(">> EF is building the model..."), [CoreEventId.ShadowPropertyCreated])
		              .EnableSensitiveDataLogging(); ;
	}

	public DbSet<KanbanCard> Cards => Set<KanbanCard>();
	public DbSet<KanbanColumn> Columns => Set<KanbanColumn>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<KanbanColumn>().HasMany(x => x.Cards).WithOne(x => x.Column);
	}
}