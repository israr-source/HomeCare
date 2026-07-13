using HomeCare.Models;
using HomeCare.Models.Identity;
using HomeCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeCare.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CustomerController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // REGISTER
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FullName = model.FullName,
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    LastLogin = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");

                    return Redirect("/Identity/Account/Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        // DASHBOARD
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Dashboard()
        {
            var customer = await _userManager.GetUserAsync(User);

            var bookings = _context.Bookings
                .Include(b => b.Service)
                .Where(b => b.CustomerId == customer.Id)
                .ToList();

            var reviews = _context.Reviews
                .Where(r => r.CustomerId == customer.Id)
                .ToList();

            var model = new CustomerDashboardViewModel
            {
                TotalBookings = bookings.Count,

                PendingBookings = bookings.Count(b => b.Status == "Pending"),

                CompletedBookings = bookings.Count(b => b.Status == "Completed"),

                CancelledBookings = bookings.Count(b => b.Status == "Cancelled"),

                TotalReviews = reviews.Count,

                RecentBookings = bookings
                    .OrderByDescending(b => b.BookingDate)
                    .Take(5)
                    .ToList()
            };

            return View(model);
        }
    }
}