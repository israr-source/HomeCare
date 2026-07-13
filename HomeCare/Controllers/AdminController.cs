using HomeCare.Models;
using HomeCare.Models.Identity;
using HomeCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // DASHBOARD
        public async Task<IActionResult> Dashboard()
        {
            var users = _userManager.Users.ToList();

            var model = new AdminDashboardViewModel
            {
                TotalCustomers = 0,
                TotalProviders = 0,

                TotalServices = _context.Services.Count(),

                TotalBookings = _context.Bookings.Count(),

                CompletedBookings = _context.Bookings
                    .Count(b => b.Status == "Completed"),

                TotalReviews = _context.Reviews.Count()
            };

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    model.TotalCustomers++;
                }

                if (await _userManager.IsInRoleAsync(user, "Provider"))
                {
                    model.TotalProviders++;
                }
            }

            return View(model);
        }

        // USER MANAGEMENT
        public async Task<IActionResult> Users(string searchTerm)
        {
            var userList = _userManager.Users.ToList();

            var users = new List<UserListViewModel>();

            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);

                users.Add(new UserListViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName ?? "N/A",
                    Email = user.Email ?? "N/A",
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                users = users
                    .Where(u =>
                        u.FullName.Contains(searchTerm,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        u.Email.Contains(searchTerm,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.SearchTerm = searchTerm;

            return View(users);
        }

        // BOOKING MANAGEMENT
        public IActionResult Bookings()
        {
            var bookings = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Service)
                .OrderByDescending(b => b.BookingDate)
                .ToList();

            ViewBag.PendingCount =
                bookings.Count(b => b.Status == "Pending");

            ViewBag.AcceptedCount =
                bookings.Count(b => b.Status == "Accepted");

            ViewBag.CompletedCount =
                bookings.Count(b => b.Status == "Completed");

            ViewBag.CancelledCount =
                bookings.Count(b => b.Status == "Cancelled");

            return View(bookings);
        }
    }
}