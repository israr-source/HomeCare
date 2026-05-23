using Microsoft.AspNetCore.Mvc;

namespace HomeCare.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult RegisterOptions()
        {
            return View();
        }
    }
}