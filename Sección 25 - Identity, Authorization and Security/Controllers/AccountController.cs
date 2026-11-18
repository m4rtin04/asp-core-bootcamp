using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sección_25___Identity__Authorization_and_Security.DTO;
using Sección_25___Identity__Authorization_and_Security.Models;

namespace Sección_25___Identity__Authorization_and_Security.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            var user = new ApplicationUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password!);

            if (!result.Succeeded)
            {
                // Acá caen, por ejemplo, las contraseñas que no cumplen las reglas
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerDTO);
            }

            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(loginDTO);
            }

            // El UserName es el email, por eso se puede loguear con él
            var result = await _signInManager.PasswordSignInAsync(
                loginDTO.Email!, loginDTO.Password!, loginDTO.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // IsLocalUrl evita que alguien arme un link que te mande a otro sitio
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

           
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            ViewBag.ReturnUrl = returnUrl;
            return View(loginDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Lo llama el atributo [Remote] de RegisterDTO.Email
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            return Json(user == null);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(new ProfileDTO { DisplayName = user.DisplayName });
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileDTO profileDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(profileDTO);
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            user.DisplayName = profileDTO.DisplayName;
            await _userManager.UpdateAsync(user);

            TempData["Message"] = "Profile updated";
            return RedirectToAction(nameof(Profile));
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
