using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Sección_23___Solid_Principles.Controllers;
using Sección_23___Solid_Principles.Exceptions;
using Sección_23___Solid_Principles.Models;
using Sección_23___Solid_Principles.Options;
using Sección_23___Solid_Principles.RepositoryContracts;
using Sección_23___Solid_Principles.ServiceContracts;
using Sección_23___Solid_Principles.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class StocksControllerTest
    {
        [Fact]
        public async Task Explore_NullStock_ReturnsViewWithListOfStocks()
        {
            var finnhubMock = new Mock<IFinnhubStocksService>();
            finnhubMock.Setup(s => s.GetStocks()).ReturnsAsync(new List<Dictionary<string, string>>
        {
            new() { ["symbol"] = "AAPL", ["description"] = "APPLE INC" },
            new() { ["symbol"] = "MSFT", ["description"] = "MICROSOFT CORP" },
            new() { ["symbol"] = "ZZZZ", ["description"] = "NOT POPULAR INC" }
        });

            var options = Options.Create(new TradingOptions { Top25PopularStocks = "AAPL,MSFT" });
            var controller = new StocksController(options, finnhubMock.Object,
                                      NullLogger<StocksController>.Instance);

            IActionResult result = await controller.Explore(null);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.ViewData.Model.Should().BeAssignableTo<List<Stock>>().Subject;

            // solo quedaron las populares
            model.Select(s => s.StockSymbol).Should().BeEquivalentTo(new[] { "AAPL", "MSFT" });
        }
    }
}
