using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.DTO;

namespace Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Models
{
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();

        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();
    }
}
