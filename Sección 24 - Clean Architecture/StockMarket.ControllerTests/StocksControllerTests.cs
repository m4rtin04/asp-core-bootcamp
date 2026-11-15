using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StockMarket.Core.ServiceContracts;
using StockMarket.UI.Controllers;
using StockMarket.UI.Models;
using Xunit;

namespace StockMarket.ControllerTests;

public class StocksControllerTests
{
    [Fact]
    public async Task Explore_NullStock_ReturnsExploreViewWithListOfStocks()
    {
        // Arrange
        var serviceMock = new Mock<IFinnhubStocksService>();
        serviceMock.Setup(s => s.GetStocks()).ReturnsAsync(new List<Dictionary<string, string>>
        {
            new() { ["symbol"] = "AAPL", ["description"] = "APPLE INC" },
            new() { ["symbol"] = "XXXX", ["description"] = "NOT POPULAR" }
        });
        var options = Options.Create(new TradingOptions { Top25PopularStocks = "AAPL,MSFT" });
        var controller = new StocksController(serviceMock.Object, options, Mock.Of<ILogger<StocksController>>());

        // Act
        var result = await controller.Explore(null);

        // Assert
        var view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.Model.Should().BeAssignableTo<List<Stock>>();
        var model = (List<Stock>)view.ViewData.Model!;
        model.Should().ContainSingle(s => s.StockSymbol == "AAPL");
    }
}
