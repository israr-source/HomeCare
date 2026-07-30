using HomeCare.Models;
using HomeCare.Models.Identity;
using HomeCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.Categories = new SelectList(_context.ServiceCategories, "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ProviderRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_context.ServiceCategories.Any(c => c.Name == model.ServiceType))
                {
                    ModelState.AddModelError("ServiceType", "The selected category does not exist.");
                }
            }

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

            ViewBag.Categories = new SelectList(_context.ServiceCategories, "Name", "Name", model.ServiceType);
            return View(model);
        }

        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Dashboard()
        {
            var provider = await _userManager.GetUserAsync(User);

            var myBookings = _context.Bookings
                .Where(b => b.ProviderId == provider.Id)
                .ToList();

            var pendingJobsCount = await _context.Bookings
                .Include(b => b.Service)
                    .ThenInclude(s => s.ServiceCategory)
                .CountAsync(b => b.Status == "Pending" && b.ProviderId == null && b.Service.ServiceCategory.Name == provider.ServiceType);

            var reviews = _context.Reviews
                .Where(r => r.ProviderId == provider.Id)
                .ToList();

            var model = new ProviderDashboardViewModel
            {
                PendingJobs = pendingJobsCount,
                AcceptedJobs = myBookings.Count(b => b.Status == "Accepted"),
                CompletedJobs = myBookings.Count(b => b.Status == "Completed"),
                TotalReviews = reviews.Count,
                AverageRating = reviews.Any()
                    ? reviews.Average(r => r.Rating)
                    : 0
            };

            return View(model);
        }

        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> AvailableJobs()
        {
            var provider = await _userManager.GetUserAsync(User);

            var bookings = await _context.Bookings
                .Include(b => b.Service)
                    .ThenInclude(s => s.ServiceCategory)
                .Include(b => b.Customer)
                .Where(b => b.Status == "Pending" && b.ProviderId == null && b.Service.ServiceCategory.Name == provider.ServiceType)
                .ToListAsync();

            return View(bookings);
        }

        [Authorize(Roles = "Provider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptJob(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Service)
                    .ThenInclude(s => s.ServiceCategory)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var provider = await _userManager.GetUserAsync(User);

            if (booking.Status != "Pending" || booking.ProviderId != null || booking.Service.ServiceCategory.Name != provider.ServiceType)
            {
                TempData["ErrorMessage"] = "This job is no longer available or does not match your service category.";
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

        [Authorize(Roles = "Provider")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProviderProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ServiceType = user.ServiceType
            };

            ViewBag.Categories = new SelectList(_context.ServiceCategories, "Name", "Name", model.ServiceType);
            return View(model);
        }

        [Authorize(Roles = "Provider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProviderProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            model.Email = user.Email; // Keep it unchanged

            if (ModelState.IsValid)
            {
                if (!_context.ServiceCategories.Any(c => c.Name == model.ServiceType))
                {
                    ModelState.AddModelError("ServiceType", "The selected category does not exist.");
                }
            }

            if (ModelState.IsValid)
            {
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.ServiceType = model.ServiceType;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Profile updated successfully.";
                    return RedirectToAction(nameof(Profile));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Categories = new SelectList(_context.ServiceCategories, "Name", "Name", model.ServiceType);
            return View(model);
        }
    }
}
