using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.DTO;

namespace Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.ServiceContracts
{
    public interface IStocksService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
        Task<List<BuyOrderResponse>> GetBuyOrders();
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
