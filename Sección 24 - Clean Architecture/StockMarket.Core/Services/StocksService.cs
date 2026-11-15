using Microsoft.Extensions.Logging;
using StockMarket.Core.Domain.RepositoryContracts;
using StockMarket.Core.DTO;
using StockMarket.Core.Helpers;
using StockMarket.Core.ServiceContracts;

namespace StockMarket.Core.Services;

public class StocksService : IStocksService
{
    private readonly IStocksRepository _stocksRepository;
    private readonly ILogger<StocksService> _logger;

    public StocksService(IStocksRepository stocksRepository, ILogger<StocksService> logger)
    {
        _stocksRepository = stocksRepository;
        _logger = logger;
    }

    public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
        if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));
        ValidationHelper.ModelValidation(buyOrderRequest);

        var created = await _stocksRepository.CreateBuyOrder(buyOrderRequest.ToBuyOrder());
        _logger.LogInformation("Buy order {OrderId} created for {Symbol}", created.BuyOrderID, created.StockSymbol);
        return created.ToBuyOrderResponse();
    }

    public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
        if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));
        ValidationHelper.ModelValidation(sellOrderRequest);

        var created = await _stocksRepository.CreateSellOrder(sellOrderRequest.ToSellOrder());
        _logger.LogInformation("Sell order {OrderId} created for {Symbol}", created.SellOrderID, created.StockSymbol);
        return created.ToSellOrderResponse();
    }

    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        _logger.LogInformation("Retrieving buy orders");
        return (await _stocksRepository.GetBuyOrders()).Select(o => o.ToBuyOrderResponse()).ToList();
    }

    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
        _logger.LogInformation("Retrieving sell orders");
        return (await _stocksRepository.GetSellOrders()).Select(o => o.ToSellOrderResponse()).ToList();
    }
}
