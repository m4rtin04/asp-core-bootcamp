using AutoFixture;
using FluentAssertions;
using Moq;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.DTO;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Entities;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.RepositoryContracts;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.ServiceContracts;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Services;

namespace Tests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _service;
        private readonly Mock<IStocksRepository> _repoMock = new();
        private readonly IFixture _fixture = new Fixture();

        public StocksServiceTest()
        {
            _service = new StocksService(_repoMock.Object);
        }

        // Requests válidos generados con AutoFixture
        private BuyOrderRequest ValidBuy() => _fixture.Build<BuyOrderRequest>()
            .With(r => r.Quantity, 100u)
            .With(r => r.Price, 100d)
            .With(r => r.DateAndTimeOfOrder, new DateTime(2024, 12, 31))
            .Create();

        private SellOrderRequest ValidSell() => _fixture.Build<SellOrderRequest>()
            .With(r => r.Quantity, 100u)
            .With(r => r.Price, 100d)
            .With(r => r.DateAndTimeOfOrder, new DateTime(2024, 12, 31))
            .Create();

        #region CreateBuyOrder

        [Fact]
        public async Task CreateBuyOrder_NullRequest_ThrowsArgumentNullException()
        {
            Func<Task> action = async () => await _service.CreateBuyOrder(null);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0u)]
        [InlineData(100001u)]
        public async Task CreateBuyOrder_QuantityOutOfRange_ThrowsArgumentException(uint quantity)
        {
            var request = _fixture.Build<BuyOrderRequest>()
                .With(r => r.Quantity, quantity)
                .With(r => r.Price, 100d)
                .With(r => r.DateAndTimeOfOrder, new DateTime(2024, 12, 31))
                .Create();

            Func<Task> action = async () => await _service.CreateBuyOrder(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(0d)]
        [InlineData(10001d)]
        public async Task CreateBuyOrder_PriceOutOfRange_ThrowsArgumentException(double price)
        {
            var request = _fixture.Build<BuyOrderRequest>()
                .With(r => r.Quantity, 100u)
                .With(r => r.Price, price)
                .With(r => r.DateAndTimeOfOrder, new DateTime(2024, 12, 31))
                .Create();

            Func<Task> action = async () => await _service.CreateBuyOrder(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol_ThrowsArgumentException()
        {
            var request = _fixture.Build<BuyOrderRequest>()
                .Without(r => r.StockSymbol)
                .With(r => r.Quantity, 100u)
                .With(r => r.Price, 100d)
                .With(r => r.DateAndTimeOfOrder, new DateTime(2024, 12, 31))
                .Create();

            Func<Task> action = async () => await _service.CreateBuyOrder(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_DateOlderThan2000_ThrowsArgumentException()
        {
            var request = _fixture.Build<BuyOrderRequest>()
                .With(r => r.Quantity, 100u)
                .With(r => r.Price, 100d)
                .With(r => r.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
                .Create();

            Func<Task> action = async () => await _service.CreateBuyOrder(request);
            await action.Should().ThrowAsync<ArgumentException>();

            // si la validación falla, nunca se llega al repositorio
            _repoMock.Verify(r => r.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        [Fact]
        public async Task CreateBuyOrder_ValidRequest_ReturnsResponseWithNewGuid()
        {
            var request = ValidBuy();
            BuyOrder saved = request.ToBuyOrder();
            saved.BuyOrderID = Guid.NewGuid();

            _repoMock.Setup(r => r.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(saved);

            BuyOrderResponse response = await _service.CreateBuyOrder(request);

            response.BuyOrderID.Should().NotBe(Guid.Empty);
            response.Should().BeEquivalentTo(saved.ToBuyOrderResponse());
            _repoMock.Verify(r => r.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Once);
        }

        #endregion

        #region CreateSellOrder

        [Fact]
        public async Task CreateSellOrder_NullRequest_ThrowsArgumentNullException()
        {
            Func<Task> action = async () => await _service.CreateSellOrder(null);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0u)]
        [InlineData(100001u)]
        public async Task CreateSellOrder_QuantityOutOfRange_ThrowsArgumentException(uint quantity)
        {
            var request = _fixture.Build<SellOrderRequest>()
                .With(r => r.Quantity, quantity)
                .With(r => r.Price, 100d)
                .With(r => r.DateAndTimeOfOrder, new DateTime(2024, 12, 31))
                .Create();

            Func<Task> action = async () => await _service.CreateSellOrder(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(0d)]
        [InlineData(10001d)]
        public async Task CreateSellOrder_PriceOutOfRange_ThrowsArgumentException(double price)
        {
            var request = _fixture.Build<SellOrderRequest>()
                .With(r => r.Quantity, 100u)
                .With(r => r.Price, price)
                .With(r => r.DateAndTimeOfOrder, new DateTime(2024, 12, 31))
                .Create();

            Func<Task> action = async () => await _service.CreateSellOrder(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_NullStockSymbol_ThrowsArgumentException()
        {
            var request = _fixture.Build<SellOrderRequest>()
                .Without(r => r.StockSymbol)
                .With(r => r.Quantity, 100u)
                .With(r => r.Price, 100d)
                .With(r => r.DateAndTimeOfOrder, new DateTime(2024, 12, 31))
                .Create();

            Func<Task> action = async () => await _service.CreateSellOrder(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_DateOlderThan2000_ThrowsArgumentException()
        {
            var request = _fixture.Build<SellOrderRequest>()
                .With(r => r.Quantity, 100u)
                .With(r => r.Price, 100d)
                .With(r => r.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
                .Create();

            Func<Task> action = async () => await _service.CreateSellOrder(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_ValidRequest_ReturnsResponseWithNewGuid()
        {
            var request = ValidSell();
            SellOrder saved = request.ToSellOrder();
            saved.SellOrderID = Guid.NewGuid();

            _repoMock.Setup(r => r.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(saved);

            SellOrderResponse response = await _service.CreateSellOrder(request);

            response.SellOrderID.Should().NotBe(Guid.Empty);
            response.Should().BeEquivalentTo(saved.ToSellOrderResponse());
        }

        #endregion

        #region GetBuyOrders / GetSellOrders

        [Fact]
        public async Task GetBuyOrders_ByDefault_ReturnsEmptyList()
        {
            _repoMock.Setup(r => r.GetBuyOrders()).ReturnsAsync(new List<BuyOrder>());

            List<BuyOrderResponse> orders = await _service.GetBuyOrders();

            orders.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBuyOrders_AfterAddingOrders_ReturnsAllOfThem()
        {
            List<BuyOrder> stored = _fixture.CreateMany<BuyOrder>(3).ToList();
            _repoMock.Setup(r => r.GetBuyOrders()).ReturnsAsync(stored);

            List<BuyOrderResponse> orders = await _service.GetBuyOrders();

            orders.Should().BeEquivalentTo(stored.Select(o => o.ToBuyOrderResponse()));
        }

        [Fact]
        public async Task GetSellOrders_ByDefault_ReturnsEmptyList()
        {
            _repoMock.Setup(r => r.GetSellOrders()).ReturnsAsync(new List<SellOrder>());

            List<SellOrderResponse> orders = await _service.GetSellOrders();

            orders.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSellOrders_AfterAddingOrders_ReturnsAllOfThem()
        {
            List<SellOrder> stored = _fixture.CreateMany<SellOrder>(3).ToList();
            _repoMock.Setup(r => r.GetSellOrders()).ReturnsAsync(stored);

            List<SellOrderResponse> orders = await _service.GetSellOrders();

            orders.Should().BeEquivalentTo(stored.Select(o => o.ToSellOrderResponse()));
        }

        #endregion
    }
}
