namespace PizzaStore.Infrastructure.WebApp.Data.Repositories.Models;

using Microsoft.EntityFrameworkCore;

public partial class WebAppContext : DbContext
{
	public WebAppContext(DbContextOptions<WebAppContext> options) : base(options)
	{
	}

	public virtual DbSet<Pizza> Pizza => Set<Pizza>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Pizza>(entity =>
		{
			entity.HasIndex(e => e.Name).IsUnique();
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}