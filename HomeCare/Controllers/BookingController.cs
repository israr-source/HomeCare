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

            ViewBag.ReviewedBookingIds = _context.Reviews
                .Where(r => r.CustomerId == user.Id)
                .Select(r => r.BookingId)
                .ToList();

            return View(bookings);
        }

        // GET CREATE PAGE
        [HttpGet]
        public IActionResult Create(int? serviceId)
        {
            ViewBag.Services = new SelectList(
                _context.Services,
                "Id",
                "Name",
                serviceId);

            if (serviceId.HasValue)
            {
                ViewBag.SelectedService = _context.Services
                    .Include(s => s.ServiceCategory)
                    .FirstOrDefault(s => s.Id == serviceId.Value);
            }

            var booking = new Booking();

            if (serviceId.HasValue)
            {
                booking.ServiceId = serviceId.Value;
            }

            return View(booking);
        }

        // POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (booking.BookingDate <= DateTime.Now)
            {
                ModelState.AddModelError(
                    nameof(Booking.BookingDate),
                    "Booking date must be in the future.");
            }

            if (!await _context.Services.AnyAsync(s => s.Id == booking.ServiceId))
            {
                ModelState.AddModelError(
                    nameof(Booking.ServiceId),
                    "The selected service does not exist.");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                booking.CustomerId = user.Id;
                booking.Status = "Pending";
                booking.ProviderId = null;

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.Services = new SelectList(
                _context.Services,
                "Id",
                "Name",
                booking.ServiceId);

            ViewBag.SelectedService = _context.Services
                .Include(s => s.ServiceCategory)
                .FirstOrDefault(s => s.Id == booking.ServiceId);

            return View(booking);
        }

        // POST CANCEL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (booking.CustomerId != user.Id)
            {
                return Forbid();
            }

            if (booking.Status != "Pending")
            {
                return RedirectToAction(nameof(Index));
            }

            booking.Status = "Cancelled";
            booking.ProviderId = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
