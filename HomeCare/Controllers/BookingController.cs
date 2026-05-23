using HomeCare.Models;
using HomeCare.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeCare.Controllers
{
    [Authorize(Roles = "Customer")]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // LIST CUSTOMER BOOKINGS
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var bookings = _context.Bookings
                .Include(b => b.Service)
                .Where(b => b.CustomerId == user.Id)
                .ToList();

            return View(bookings);
        }

        // GET CREATE PAGE
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = new SelectList(
                _context.Services,
                "Id",
                "Name");

            return View();
        }

        // POST CREATE
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                booking.CustomerId = user.Id;

                booking.Status = "Pending";

                _context.Bookings.Add(booking);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.Services = new SelectList(
                _context.Services,
                "Id",
                "Name");

            return View(booking);
        }
    }
}