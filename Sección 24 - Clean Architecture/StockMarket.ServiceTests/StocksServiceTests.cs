using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockMarket.Core.Domain.Entities;
using StockMarket.Core.Domain.RepositoryContracts;
using StockMarket.Core.DTO;
using StockMarket.Core.ServiceContracts;
using StockMarket.Core.Services;
using Xunit;

namespace StockMarket.ServiceTests;

public class StocksServiceTests
{
    private readonly IStocksService _stocksService;
    private readonly Mock<IStocksRepository> _repositoryMock = new();
    private readonly IFixture _fixture = new Fixture();

    public StocksServiceTests()
    {
        _stocksService = new StocksService(_repositoryMock.Object, Mock.Of<ILogger<StocksService>>());
    }

    private BuyOrderRequest ValidBuy() => _fixture.Build<BuyOrderRequest>()
        .With(r => r.Quantity, 100u)
        .With(r => r.Price, 100d)
        .With(r => r.DateAndTimeOfOrder, DateTime.Now)
        .Create();

    private SellOrderRequest ValidSell() => _fixture.Build<SellOrderRequest>()
        .With(r => r.Quantity, 100u)
        .With(r => r.Price, 100d)
        .With(r => r.DateAndTimeOfOrder, DateTime.Now)
        .Create();

    #region CreateBuyOrder
    [Fact]
    public async Task CreateBuyOrder_NullRequest_ThrowsArgumentNullException()
    {
        Func<Task> action = async () => await _stocksService.CreateBuyOrder(null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(100001u)]
    public async Task CreateBuyOrder_InvalidQuantity_ThrowsArgumentException(uint quantity)
    {
        var request = _fixture.Build<BuyOrderRequest>()
            .With(r => r.Quantity, quantity).With(r => r.Price, 100d)
            .With(r => r.DateAndTimeOfOrder, DateTime.Now).Create();

        Func<Task> action = async () => await _stocksService.CreateBuyOrder(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Theory]
    [InlineData(0d)]
    [InlineData(10001d)]
    public async Task CreateBuyOrder_InvalidPrice_ThrowsArgumentException(double price)
    {
        var request = _fixture.Build<BuyOrderRequest>()
            .With(r => r.Quantity, 100u).With(r => r.Price, price)
            .With(r => r.DateAndTimeOfOrder, DateTime.Now).Create();

        Func<Task> action = async () => await _stocksService.CreateBuyOrder(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateBuyOrder_NullStockSymbol_ThrowsArgumentException()
    {
        var request = _fixture.Build<BuyOrderRequest>()
            .With(r => r.StockSymbol, (string?)null)
            .With(r => r.Quantity, 100u).With(r => r.Price, 100d)
            .With(r => r.DateAndTimeOfOrder, DateTime.Now).Create();

        Func<Task> action = async () => await _stocksService.CreateBuyOrder(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateBuyOrder_DateOlderThan2000_ThrowsArgumentException()
    {
        var request = _fixture.Build<BuyOrderRequest>()
            .With(r => r.Quantity, 100u).With(r => r.Price, 100d)
            .With(r => r.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31")).Create();

        Func<Task> action = async () => await _stocksService.CreateBuyOrder(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateBuyOrder_ValidRequest_ReturnsResponseWithGeneratedId()
    {
        var request = ValidBuy();
        _repositoryMock.Setup(r => r.CreateBuyOrder(It.IsAny<BuyOrder>()))
            .ReturnsAsync((BuyOrder o) => o);

        var response = await _stocksService.CreateBuyOrder(request);

        response.Should().BeOfType<BuyOrderResponse>();
        response.BuyOrderID.Should().NotBe(Guid.Empty);
        response.TradeAmount.Should().Be(request.Quantity * request.Price);
    }
    #endregion

    #region CreateSellOrder
    [Fact]
    public async Task CreateSellOrder_NullRequest_ThrowsArgumentNullException()
    {
        Func<Task> action = async () => await _stocksService.CreateSellOrder(null);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(100001u)]
    public async Task CreateSellOrder_InvalidQuantity_ThrowsArgumentException(uint quantity)
    {
        var request = _fixture.Build<SellOrderRequest>()
            .With(r => r.Quantity, quantity).With(r => r.Price, 100d)
            .With(r => r.DateAndTimeOfOrder, DateTime.Now).Create();

        Func<Task> action = async () => await _stocksService.CreateSellOrder(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Theory]
    [InlineData(0d)]
    [InlineData(10001d)]
    public async Task CreateSellOrder_InvalidPrice_ThrowsArgumentException(double price)
    {
        var request = _fixture.Build<SellOrderRequest>()
            .With(r => r.Quantity, 100u).With(r => r.Price, price)
            .With(r => r.DateAndTimeOfOrder, DateTime.Now).Create();

        Func<Task> action = async () => await _stocksService.CreateSellOrder(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateSellOrder_NullStockSymbol_ThrowsArgumentException()
    {
        var request = _fixture.Build<SellOrderRequest>()
            .With(r => r.StockSymbol, (string?)null)
            .With(r => r.Quantity, 100u).With(r => r.Price, 100d)
            .With(r => r.DateAndTimeOfOrder, DateTime.Now).Create();

        Func<Task> action = async () => await _stocksService.CreateSellOrder(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateSellOrder_DateOlderThan2000_ThrowsArgumentException()
    {
        var request = _fixture.Build<SellOrderRequest>()
            .With(r => r.Quantity, 100u).With(r => r.Price, 100d)
            .With(r => r.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31")).Create();

        Func<Task> action = async () => await _stocksService.CreateSellOrder(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateSellOrder_ValidRequest_ReturnsResponseWithGeneratedId()
    {
        var request = ValidSell();
        _repositoryMock.Setup(r => r.CreateSellOrder(It.IsAny<SellOrder>()))
            .ReturnsAsync((SellOrder o) => o);

        var response = await _stocksService.CreateSellOrder(request);

        response.Should().BeOfType<SellOrderResponse>();
        response.SellOrderID.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region GetBuyOrders
    [Fact]
    public async Task GetBuyOrders_Default_ReturnsEmptyList()
    {
        _repositoryMock.Setup(r => r.GetBuyOrders()).ReturnsAsync(new List<BuyOrder>());

        var result = await _stocksService.GetBuyOrders();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetBuyOrders_AfterCreatingOrders_ReturnsSameOrders()
    {
        var orders = _fixture.CreateMany<BuyOrder>(3).ToList();
        _repositoryMock.Setup(r => r.GetBuyOrders()).ReturnsAsync(orders);

        var result = await _stocksService.GetBuyOrders();

        result.Select(o => o.BuyOrderID).Should().BeEquivalentTo(orders.Select(o => o.BuyOrderID));
    }
    #endregion

    #region GetSellOrders
    [Fact]
    public async Task GetSellOrders_Default_ReturnsEmptyList()
    {
        _repositoryMock.Setup(r => r.GetSellOrders()).ReturnsAsync(new List<SellOrder>());

        var result = await _stocksService.GetSellOrders();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSellOrders_AfterCreatingOrders_ReturnsSameOrders()
    {
        var orders = _fixture.CreateMany<SellOrder>(3).ToList();
        _repositoryMock.Setup(r => r.GetSellOrders()).ReturnsAsync(orders);

        var result = await _stocksService.GetSellOrders();

        result.Select(o => o.SellOrderID).Should().BeEquivalentTo(orders.Select(o => o.SellOrderID));
    }
    #endregion
}
