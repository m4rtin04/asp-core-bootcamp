using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Sección_21___Filters.Entities;
using Sección_21___Filters.Options;
using Sección_21___Filters.Repositories;
using Sección_21___Filters.RepositoryContracts;
using Sección_21___Filters.ServiceContracts;
using Sección_21___Filters.Services;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//Serilog configuration
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

//options pattern
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddDbContext<StockMarketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios (RL)
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddScoped<IStocksRepository, StocksRepository>();

// Servicios (BLL)
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();

var app = builder.Build();

RotativaConfiguration.Setup(app.Environment.WebRootPath, wkhtmltopdfRelativePath: "Rotativa");

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = new List<CultureInfo> { new("en-US") },
    SupportedUICultures = new List<CultureInfo> { new("en-US") }
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => Results.Redirect("/Stocks/Explore"));


app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program { }