using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace StockMarket.IntegrationTests;

// Requiere el token de Finnhub (user-secrets o variable de entorno FinnhubToken) y conexión a internet.
public class TradeControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TradeControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Index_MSFT_ReturnsHtmlWithPriceElement()
    {
        HttpResponseMessage response = await _client.GetAsync("/Trade/Index/MSFT");

        response.IsSuccessStatusCode.Should().BeTrue();

        var document = new HtmlDocument();
        document.LoadHtml(await response.Content.ReadAsStringAsync());

        var priceNodes = document.DocumentNode.SelectNodes(
            "//*[contains(concat(' ', normalize-space(@class), ' '), ' price ')]");

        priceNodes.Should().NotBeNullOrEmpty();
    }
}
