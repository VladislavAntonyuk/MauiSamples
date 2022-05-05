namespace PizzaStore.Infrastructure.WebApp.Data;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.Models;

public class WebAppContextFactory : IDesignTimeDbContextFactory<WebAppContext>
{
	public WebAppContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<WebAppContext>();
		const string connectionString = "server=localhost;port=3306;database=PizzaStore;user=root;password=password";
		optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder =>
		{
			builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
		});
		return new WebAppContext(optionsBuilder.Options);
	}
}