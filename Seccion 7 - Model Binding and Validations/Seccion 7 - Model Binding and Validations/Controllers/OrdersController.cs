using Microsoft.AspNetCore.Mvc;
using Seccion_7___Model_Binding_and_Validations.Models;

namespace Seccion_7___Model_Binding_and_Validations.Controllers
{
    public class OrdersController : Controller
    {   
        [HttpPost("/order")]
        public IActionResult CreateOrder([Bind(nameof(Order.OrderDate), nameof(Order.InvoicePrice), nameof(Order.Products))] Order order)
        {
            if(!ModelState.IsValid)
            {
                var errors = String.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                return BadRequest(errors);
            }

            Random random = new Random();
            int orderNo = random.Next(1000, 9999);
            order.OrderNo = orderNo;

            return Json($"New orden number: {order.OrderNo}");
        }
    }
}
