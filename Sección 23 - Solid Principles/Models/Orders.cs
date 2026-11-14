using Sección_23___Solid_Principles.DTO;
using System.Collections.Generic;

namespace Sección_23___Solid_Principles.Models
{
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();

        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();
    }
}
