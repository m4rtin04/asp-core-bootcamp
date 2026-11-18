using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sección_25___Identity__Authorization_and_Security.DTO;
using Sección_25___Identity__Authorization_and_Security.Models;

namespace Sección_25___Identity__Authorization_and_Security.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var rows = new List<UserRowDTO>();

            foreach (ApplicationUser user in _userManager.Users.OrderBy(u => u.Email).ToList())
            {
                rows.Add(new UserRowDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    IsAdmin = await _userManager.IsInRoleAsync(user, "Admin")
                });
            }

            return View(rows);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAdmin(Guid id)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            // Evita que el último admin se quite el rol a sí mismo
            if (user.Id.ToString() == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You can't change your own role";
                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
