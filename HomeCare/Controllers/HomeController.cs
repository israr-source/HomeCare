using HomeCare.Models;
using HomeCare.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HomeCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new HomeIndexViewModel
            {
                FeaturedServices = _context.Services
                    .Include(s => s.ServiceCategory)
                    .OrderBy(s => s.Name)
                    .Take(6)
                    .ToList(),

                Categories = _context.ServiceCategories
                    .OrderBy(c => c.Name)
                    .Take(6)
                    .ToList(),

                TotalServices = _context.Services.Count(),

                TotalCategories = _context.ServiceCategories.Count(),

                CompletedBookings = _context.Bookings
                    .Count(b => b.Status == "Completed"),

                TotalReviews = _context.Reviews.Count()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
