using Sección_15___xUnit.DTO;
using Sección_15___xUnit.Entities;
using Sección_15___xUnit.Helpers;
using Sección_15___xUnit.ServiceContracts;

namespace Sección_15___xUnit.Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders = new();
        private readonly List<SellOrder> _sellOrders = new();

        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            ValidationHelpers.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            _buyOrders.Add(buyOrder);
            return Task.FromResult(buyOrder.ToBuyOrderResponse());
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelpers.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            _sellOrders.Add(sellOrder);

            return Task.FromResult(sellOrder.ToSellOrderResponse());
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return Task.FromResult(_buyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList());
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            return Task.FromResult(_sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList());
        }
    }
}

        
 
