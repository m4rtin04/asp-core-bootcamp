using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace StockMarket.IntegrationTests;

// Requiere el token de Finnhub (user-secrets o variable de entorno FinnhubToken) y conexión a internet.
public class TradeControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TradeControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Index_ToReturnView()
    {
        var response = await _client.GetAsync("/Trade/Index/MSFT");

        response.Should().BeSuccessful();

        var html = new HtmlDocument();
        html.LoadHtml(await response.Content.ReadAsStringAsync());

        html.DocumentNode.SelectSingleNode("//*[contains(@class,'price')]").Should().NotBeNull();
    }
}
