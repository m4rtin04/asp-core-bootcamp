using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sección_20___Logging_and_Serilog.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


namespace Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IFinnhubRepository>();
                services.AddScoped<IFinnhubRepository, FakeFinnhubRepository>();
            });
        }

        public class FakeFinnhubRepository : IFinnhubRepository
        {
            public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
                => Task.FromResult(JsonSerializer.Deserialize<Dictionary<string, object>>(
                    "{\"name\":\"Microsoft Corp\",\"ticker\":\"MSFT\"}"));

            public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
                => Task.FromResult(JsonSerializer.Deserialize<Dictionary<string, object>>("{\"c\":235.87}"));

            public Task<List<Dictionary<string, string>>?> GetStocks()
                => Task.FromResult<List<Dictionary<string, string>>?>(new());

            public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
                => Task.FromResult<Dictionary<string, object>?>(new());
        }
    }
}
