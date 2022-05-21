namespace PizzaStore.Infrastructure.Mobile.Data;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.Models;

public class MobileAppContextFactory : IDesignTimeDbContextFactory<MobileAppContext>
{
	public MobileAppContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<MobileAppContext>();
		const string connectionString = "Filename=PizzaStore.db";
		optionsBuilder.UseSqlite(connectionString, builder =>
		{
			builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
		});
		return new MobileAppContext(optionsBuilder.Options);
	}
}