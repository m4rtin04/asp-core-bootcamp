using Microsoft.AspNetCore.Localization;
using Rotativa.AspNetCore;
using StockMarket.UI.Middleware;
using StockMarket.UI.StartupExtensions;
using System.Globalization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.UseDeveloperExceptionPage();
}
else
{
app.UseExceptionHandler("/Home/Error");
app.UseExceptionHandlingMiddleware();
}

app.UseRequestLocalization(new RequestLocalizationOptions
{
DefaultRequestCulture = new RequestCulture("en-US"),
SupportedCultures = new List<CultureInfo> { new("en-US") },
SupportedUICultures = new List<CultureInfo> { new("en-US") }
});

app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseRouting();

app.MapGet("/", () => Results.Redirect("/Stocks/Explore"));
app.MapControllers();

RotativaConfiguration.Setup(app.Environment.WebRootPath, wkhtmltopdfRelativePath: "Rotativa");

app.Run();

public partial class Program { }