using HomeCare.Models;
using HomeCare.Models.Identity;
using HomeCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Users()
        {
            // Load all users first to avoid DataReader issues
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

            return View(users);
        }
    }
}