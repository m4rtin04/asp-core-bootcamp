using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.DTO;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Entities;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Helpers;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.RepositoryContracts;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.ServiceContracts;

namespace Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _repository;

        public StocksService(IStocksRepository repository) => _repository = repository;

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            BuyOrder created = await _repository.CreateBuyOrder(buyOrder);
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
