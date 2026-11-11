namespace Sección_20___Logging_and_Serilog.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public double Price { get; set; }
        public uint Quantity { get; set; }
    }
}
