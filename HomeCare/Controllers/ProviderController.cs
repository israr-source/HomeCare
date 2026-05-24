using HomeCare.Models.Identity;
using HomeCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeCare.Controllers
{
    public class ProviderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProviderController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ProviderRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var provider = new ApplicationUser
                {
                    FullName = model.FullName,
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    LastLogin = DateTime.Now
                };

                var result = await _userManager.CreateAsync(provider, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(provider, "Provider");

                    return Redirect("/Identity/Account/Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Provider")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}