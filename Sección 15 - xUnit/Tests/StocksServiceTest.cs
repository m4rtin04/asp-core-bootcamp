using Sección_15___xUnit.DTO;
using Sección_15___xUnit.ServiceContracts;
using Sección_15___xUnit.Services;
using Xunit;


namespace Sección_15___xUnit.Tests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;

        // xUnit crea una instancia nueva por cada test, así que la lista siempre empieza vacía
        public StocksServiceTest()
        {
            _stocksService = new StocksService();
        }

        private static BuyOrderRequest ValidBuy() => new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2024-12-31"),
            Quantity = 1,
            Price = 1
        };

        private static SellOrderRequest ValidSell() => new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2024-12-31"),
            Quantity = 1,
            Price = 1
        };

        #region CreateBuyOrder
        [Fact]
        public async Task CreateBuyOrder_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100001)]
        public async Task CreateBuyOrder_QuantityOutOfRange_ThrowsArgumentException(uint quantity)
        {
            // Arrange
            var buyOrderRequest = ValidBuy();
            buyOrderRequest.Quantity = quantity;
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10001)]
        public async Task CreateBuyOrder_PriceOutOfRange_ThrowsArgumentException(double price)
        {
            // Arrange
            var buyOrderRequest = ValidBuy();
            buyOrderRequest.Price = price;
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol_ThrowsArgumentException()
        {
            // Arrange
            var buyOrderRequest = ValidBuy();
            buyOrderRequest.StockSymbol = null;
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }

        [Fact]
        public async Task CreateBuyOrder_DateOlderThan2000_ThrowsArgumentException()
        {
            // Arrange
            var buyOrderRequest = ValidBuy();
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Parse("1999-12-31");
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderRequest));
        }
        [Fact]
        public async Task CreateBuyOrder_ValidRequest_ReturnsResponseWithNewGuid()
        {
            // Arrange
            var buyOrderRequest = ValidBuy();
            // Act
            var response = await _stocksService.CreateBuyOrder(buyOrderRequest);
            // Assert
            Assert.NotEqual(Guid.Empty, response.BuyOrderID);
            Assert.Equal("MSFT", response.StockSymbol);
            Assert.Equal(1, response.TradeAmount);
        }

        #endregion

        #region CreateSellOrder
        [Fact]
        public async Task CreateSellOrder_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100001)]
        public async Task CreateSellOrder_QuantityOutOfRange_ThrowsArgumentException(uint quantity)
        {
            var request = ValidSell();
            request.Quantity = quantity;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _stocksService.CreateSellOrder(request));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10001)]
        public async Task CreateSellOrder_PriceOutOfRange_ThrowsArgumentException(double price)
        {
            var request = ValidSell();
            request.Price = price;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _stocksService.CreateSellOrder(request));
        }

        [Fact]
        public async Task CreateSellOrder_NullStockSymbol_ThrowsArgumentException()
        {
            var request = ValidSell();
            request.StockSymbol = null;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _stocksService.CreateSellOrder(request));
        }

        [Fact]
        public async Task CreateSellOrder_DateOlderThan2000_ThrowsArgumentException()
        {
            var request = ValidSell();
            request.DateAndTimeOfOrder = DateTime.Parse("1999-12-31");

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _stocksService.CreateSellOrder(request));
        }

        [Fact]
        public async Task CreateSellOrder_ValidRequest_ReturnsResponseWithNewGuid()
        {
            var response = await _stocksService.CreateSellOrder(ValidSell());

            Assert.NotEqual(Guid.Empty, response.SellOrderID);
            Assert.Equal("MSFT", response.StockSymbol);
            Assert.Equal(1, response.TradeAmount);
        }
        #endregion

        #region GetBuyOrders
        [Fact]
        public async Task GetBuyOrders_ByDefault_ReturnsEmptyList()
        {
            var orders = await _stocksService.GetBuyOrders();

            Assert.Empty(orders);
        }

        [Fact]
        public async Task GetBuyOrders_AfterAddingOrders_ReturnsAllOfThem()
        {
            var created = new List<BuyOrderResponse>
        {
            await _stocksService.CreateBuyOrder(ValidBuy()),
            await _stocksService.CreateBuyOrder(ValidBuy()),
            await _stocksService.CreateBuyOrder(ValidBuy())
        };

            var orders = await _stocksService.GetBuyOrders();

            Assert.Equal(created.Count, orders.Count);
            foreach (var expected in created)
                Assert.Contains(orders, o => o.BuyOrderID == expected.BuyOrderID);
        }
        #endregion

        #region GetSellOrders
        [Fact]
        public async Task GetSellOrders_ByDefault_ReturnsEmptyList()
        {
            var orders = await _stocksService.GetSellOrders();
            Assert.Empty(orders);
        }

        [Fact]
        public async Task GetSellOrders_AfterAddingOrders_ReturnsAllOfThem()
        {
            var created = new List<SellOrderResponse>
        {
            await _stocksService.CreateSellOrder(ValidSell()),
            await _stocksService.CreateSellOrder(ValidSell()),
            await _stocksService.CreateSellOrder(ValidSell())
        };
            var orders = await _stocksService.GetSellOrders();
            Assert.Equal(created.Count, orders.Count);
            foreach (var expected in created)
                Assert.Contains(orders, o => o.SellOrderID == expected.SellOrderID);
        }
        #endregion
    }
}
