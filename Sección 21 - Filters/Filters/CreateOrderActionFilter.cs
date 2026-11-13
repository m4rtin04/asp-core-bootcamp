using Microsoft.AspNetCore.Mvc.Filters;
using Sección_21___Filters.Controllers;
using Sección_21___Filters.DTO;
using Sección_21___Filters.Models;

namespace Sección_21___Filters.Filters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<CreateOrderActionFilter> _logger;
        private readonly IConfiguration _configuration;

        public CreateOrderActionFilter(ILogger<CreateOrderActionFilter> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Before logic
            if (context.Controller is TradeController tradeController
                && context.ActionArguments.TryGetValue("orderRequest", out var argument)
                && argument is OrderRequest orderRequest)
            {
                // Se asigna la fecha real y se vuelve a validar.
                orderRequest.DateAndTimeOfOrder = DateTime.Now;
                tradeController.ModelState.Clear();
                tradeController.TryValidateModel(orderRequest);

                if (!tradeController.ModelState.IsValid)
                {
                    _logger.LogWarning("Order rejected for {StockSymbol}: {ErrorCount} validation error(s)",
                        orderRequest.StockSymbol, tradeController.ModelState.ErrorCount);

                    tradeController.ViewBag.Errors = tradeController.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    tradeController.ViewBag.FinnhubToken = _configuration["FinnhubToken"];

                    var stockTrade = new StockTrade
                    {
                        StockSymbol = orderRequest.StockSymbol,
                        StockName = orderRequest.StockName,
                        Quantity = orderRequest.Quantity,
                        Price = orderRequest.Price
                    };

                    // Cortocircuito: se devuelve la vista y la acción nunca se ejecuta
                    context.Result = tradeController.View("Index", stockTrade);
                    return;
                }
            }

            await next();   // sin errores: sigue el pipeline

            // After logic: no se necesita
        }
    }
}
