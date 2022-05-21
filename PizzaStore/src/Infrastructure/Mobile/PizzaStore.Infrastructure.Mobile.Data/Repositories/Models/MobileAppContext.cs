namespace PizzaStore.Infrastructure.Mobile.Data.Repositories.Models;

using Microsoft.EntityFrameworkCore;

public partial class MobileAppContext : DbContext
{
	public MobileAppContext(DbContextOptions<MobileAppContext> options) : base(options)
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