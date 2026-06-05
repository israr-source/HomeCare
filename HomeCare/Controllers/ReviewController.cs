using HomeCare.Models;
using HomeCare.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeCare.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create(int bookingId)
        {
            var review = new Review
            {
                BookingId = bookingId
            };

            return View(review);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Review review)
        {
            var user = await _userManager.GetUserAsync(User);

            var booking = await _context.Bookings.FindAsync(review.BookingId);

            if (booking == null)
            {
                return NotFound();
            }

            review.CustomerId = user.Id;
            review.ProviderId = booking.ProviderId;

            _context.Reviews.Add(review);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Booking");
        }
    }
}