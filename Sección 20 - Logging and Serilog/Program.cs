using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Sección_20___Logging_and_Serilog.Entities;
using Sección_20___Logging_and_Serilog.Options;
using Sección_20___Logging_and_Serilog.Repositories;
using Sección_20___Logging_and_Serilog.RepositoryContracts;
using Sección_20___Logging_and_Serilog.ServiceContracts;
using Sección_20___Logging_and_Serilog.Services;
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