using Microsoft.AspNetCore.Localization;
using MudBlazor.Services;
using PizzaStore.Application.Configuration;
using PizzaStore.Infrastructure.WebApp.Business;
using PizzaStore.Infrastructure.WebApp.Data.Configuration;
using PizzaStore.WebApp.Extensions;
using PizzaStore.WebApp.Models;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructureData("server=localhost;port=3306;database=PizzaStore;user=root;password=password");
builder.Services.AddInfrastructureBusiness();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddMudServices();
builder.Services.AddI18nText();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = Enum.GetValues<Language>().Select(x => x.GetDescription()).ToArray();
	options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
	options.AddSupportedCultures(supportedCultures);
	options.AddSupportedUICultures(supportedCultures);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRequestLocalization();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();