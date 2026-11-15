using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StockMarket.Core.DTO;
using StockMarket.UI.Controllers;
using StockMarket.UI.Models;

namespace StockMarket.UI.Filters;

public class CreateOrderActionFilter : IAsyncActionFilter
{
    private readonly ILogger<CreateOrderActionFilter> _logger;

    public CreateOrderActionFilter(ILogger<CreateOrderActionFilter> logger) => _logger = logger;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is TradeController tradeController
            && context.ActionArguments.TryGetValue("orderRequest", out var orderRequest)
            && orderRequest != null)
        {
            // La fecha se asigna aquí (después del model binding) y se vuelve a validar el modelo.
            string? symbol = null, name = null;
            uint quantity = 0;
            double price = 0;

            switch (orderRequest)
            {
                case BuyOrderRequest buy:
                    buy.DateAndTimeOfOrder = DateTime.Now;
                    (symbol, name, quantity, price) = (buy.StockSymbol, buy.StockName, buy.Quantity, buy.Price);
                    break;
                case SellOrderRequest sell:
                    sell.DateAndTimeOfOrder = DateTime.Now;
                    (symbol, name, quantity, price) = (sell.StockSymbol, sell.StockName, sell.Quantity, sell.Price);
                    break;
            }

            tradeController.ModelState.Clear();
            tradeController.TryValidateModel(orderRequest);

            if (!tradeController.ModelState.IsValid)
            {
                _logger.LogWarning("Invalid order request for {Symbol}", symbol);

                tradeController.ViewBag.Errors = tradeController.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var model = new StockTrade { StockSymbol = symbol, StockName = name, Quantity = quantity, Price = price };
                context.Result = tradeController.View("Index", model);   // corta el pipeline
                return;
            }
        }

        await next();   // sin errores: continúa con el siguiente filtro/acción
    }
}
