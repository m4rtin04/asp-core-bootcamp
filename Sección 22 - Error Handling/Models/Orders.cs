using Sección_22___Error_Handling.DTO;
using System.Collections.Generic;

namespace Sección_22___Error_Handling.Models
{
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();

        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();
    }
}
