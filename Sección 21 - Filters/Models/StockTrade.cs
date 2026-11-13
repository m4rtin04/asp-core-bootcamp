namespace Sección_21___Filters.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public double Price { get; set; }
        public uint Quantity { get; set; }
    }
}
