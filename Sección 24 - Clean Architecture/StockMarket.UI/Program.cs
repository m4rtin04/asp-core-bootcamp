using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using StockMarket.Core.Domain.RepositoryContracts;
using StockMarket.Core.ServiceContracts;
using StockMarket.Core.Services;
using StockMarket.Infrastructure.DatabaseContext;
using StockMarket.Infrastructure.Repositories;
using StockMarket.UI.Middleware;
using StockMarket.UI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// Options pattern
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

// Infrastructure
builder.Services.AddDbContext<StockMarketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddScoped<IStocksRepository, StocksRepository>();

// Core
builder.Services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
builder.Services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
builder.Services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();
builder.Services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
builder.Services.AddScoped<IStocksService, StocksService>();

var app = builder.Build();

// Cultura fija: el punto decimal es el que usan Finnhub y el JavaScript de la vista.
var culture = new CultureInfo("en-US");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = new List<CultureInfo> { culture },
    SupportedUICultures = new List<CultureInfo> { culture }
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");   // muestra la vista amigable
    app.UseExceptionHandlingMiddleware();     // registra y relanza
}

// Rotativa necesita wkhtmltopdf.exe en wwwroot/Rotativa
Rotativa.AspNetCore.RotativaConfiguration.Setup(app.Environment.WebRootPath, wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program { }   // necesario para WebApplicationFactory<Program>
