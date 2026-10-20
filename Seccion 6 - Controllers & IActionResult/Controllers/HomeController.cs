using Microsoft.AspNetCore.Mvc;

namespace Seccion_6___Controllers___IActionResult.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Best Bank", "text/plain");
        }

        [Route("/account-details")]
        public IActionResult Details()
        {
            var details = new
            {
                accountNumber = 1001,
                accountHolderName = "Example Name",
                currentBalance = 5000
            };
            return Json(details);
        }

        [Route("/account-statement")]
        public IActionResult Statement()
        {
            return File("~/statement.pdf", "application/pdf");
        }

        [Route("/get-current-balance/{accountNumber:int?}")]
        public IActionResult GetCurrentBalance()
        {
            if(HttpContext.Request.RouteValues.TryGetValue("accountNumber", out object? value) && value != null)
            {
                string accountNumberStr = value.ToString()!;

                if(int.TryParse(accountNumberStr, out int accountNumber))
                {
                    if(accountNumber != 1001)
                    {
                        return BadRequest("Account number should be 1001.");
                    }    

                    return Content("5000", "text/plain");
                }
                else
                {
                    return BadRequest("Invalid account number format.");
                }
            }
            else
            {
                return NotFound("Account number should be supplied in the URL.");
            }
        }
    }
}
