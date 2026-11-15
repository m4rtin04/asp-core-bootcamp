using Microsoft.EntityFrameworkCore;
using StockMarket.Core.Domain.RepositoryContracts;
using StockMarket.Core.ServiceContracts;
using StockMarket.Core.Services;
using StockMarket.Infrastructure.DatabaseContext;
using StockMarket.Infrastructure.Repositories;
using StockMarket.UI.Options;

namespace StockMarket.UI.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();

            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));

            services.AddDbContext<StockMarketDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositorios
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IStocksRepository, StocksRepository>();

            // Servicios
            services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
            services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
            services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();
            services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
            services.AddScoped<IStocksService, StocksService>();

            return services;
        }
    }
}
