using Sección_21___Filters.DTO;
using Sección_21___Filters.Entities;
using Sección_21___Filters.Helpers;
using Sección_21___Filters.RepositoryContracts;
using Sección_21___Filters.ServiceContracts;

namespace Sección_21___Filters.Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _repository;
        private readonly ILogger<StocksService> _logger;

        public StocksService(IStocksRepository repository, ILogger<StocksService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            _logger.LogInformation("Creating buy order for {StockSymbol}", buyOrderRequest.StockSymbol);

            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            BuyOrder created = await _repository.CreateBuyOrder(buyOrder);

            _logger.LogInformation("Buy order {BuyOrderID} saved", created.BuyOrderID);

            return created.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            SellOrder created = await _repository.CreateSellOrder(sellOrder);
            return created.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> orders = await _repository.GetBuyOrders();
            return orders.Select(o => o.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> orders = await _repository.GetSellOrders();
            return orders.Select(o => o.ToSellOrderResponse()).ToList();
        }

    }
}
