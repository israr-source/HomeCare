using HomeCare.Models;
using HomeCare.Models.Identity;
using HomeCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeCare.Controllers
{
    public class ProviderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProviderController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
                    ServiceType = model.ServiceType,
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
        public async Task<IActionResult> Dashboard()
        {
            var provider = await _userManager.GetUserAsync(User);

            var bookings = _context.Bookings
                .Where(b => b.ProviderId == provider.Id)
                .ToList();

            var reviews = _context.Reviews
                .Where(r => r.ProviderId == provider.Id)
                .ToList();

            var model = new ProviderDashboardViewModel
            {
                PendingJobs = bookings.Count(b => b.Status == "Pending"),

                AcceptedJobs = bookings.Count(b => b.Status == "Accepted"),

                CompletedJobs = bookings.Count(b => b.Status == "Completed"),

                TotalReviews = reviews.Count,

                AverageRating = reviews.Any()
                    ? reviews.Average(r => r.Rating)
                    : 0
            };

            return View(model);
        }

        [Authorize(Roles = "Provider")]
        public IActionResult AvailableJobs()
        {
            var bookings = _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.Customer)
                .Where(b => b.Status == "Pending")
                .ToList();

            return View(bookings);
        }

        [Authorize(Roles = "Provider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptJob(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            var provider = await _userManager.GetUserAsync(User);

            if (booking.Status != "Pending" || booking.ProviderId != null)
            {
                return RedirectToAction(nameof(AvailableJobs));
            }

            booking.ProviderId = provider.Id;
            booking.Status = "Accepted";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AvailableJobs));
        }

        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> MyJobs()
        {
            var provider = await _userManager.GetUserAsync(User);

            var bookings = _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.Customer)
                .Where(b => b.ProviderId == provider.Id)
                .ToList();

            return View(bookings);
        }

        [Authorize(Roles = "Provider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteJob(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            var provider = await _userManager.GetUserAsync(User);

            if (booking.ProviderId != provider.Id)
            {
                return Forbid();
            }

            if (booking.Status != "Accepted")
            {
                return RedirectToAction(nameof(MyJobs));
            }

            booking.Status = "Completed";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyJobs));
        }

        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Reviews()
        {
            var provider = await _userManager.GetUserAsync(User);

            var reviews = _context.Reviews
                .Include(r => r.Customer)
                .Where(r => r.ProviderId == provider.Id)
                .ToList();

            ViewBag.TotalReviews = reviews.Count;

            ViewBag.AverageRating =
                reviews.Any()
                ? reviews.Average(r => r.Rating)
                : 0;

            return View(reviews);
        }
    }
}
