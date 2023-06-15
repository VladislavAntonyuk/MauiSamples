namespace KanbanBoard.Models;

using Microsoft.EntityFrameworkCore;

public class KanbanBoardDbContext : DbContext
{
	private readonly IPath path;

	public KanbanBoardDbContext(IPath path, DbContextOptions<KanbanBoardDbContext> options)
		: base(options)
	{
		this.path = path;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.UseSqlite($"Filename={path.GetDatabasePath()}");
	}

	public DbSet<Card> Cards { get; set; } = null!;
	public DbSet<Column> Columns { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<Column>().HasMany(x => x.Cards).WithOne(x => x.Column);
	}
}