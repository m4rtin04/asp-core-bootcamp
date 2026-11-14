using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using Tests;

namespace Tests
{
    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
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
}