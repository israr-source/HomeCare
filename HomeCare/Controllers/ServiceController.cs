using HomeCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeCare.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST SERVICES (PUBLIC)
        public IActionResult Index(
            string searchTerm,
            int? categoryId)
        {
            var services = _context.Services
                .Include(s => s.ServiceCategory)
                .AsQueryable();

            // Search by service name
            if (!string.IsNullOrEmpty(searchTerm))
            {
                services = services.Where(s =>
                    s.Name.Contains(searchTerm));
            }

            // Filter by category
            if (categoryId.HasValue)
            {
                services = services.Where(s =>
                    s.ServiceCategoryId == categoryId);
            }

            ViewBag.Categories =
                _context.ServiceCategories.ToList();

            ViewBag.SearchTerm = searchTerm;

            ViewBag.CategoryId = categoryId;

            return View(services.ToList());
        }

        // SERVICE DETAILS (PUBLIC)
        public IActionResult Details(int id)
        {
            var service = _context.Services
                .Include(s => s.ServiceCategory)
                .FirstOrDefault(s => s.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET CREATE PAGE (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                _context.ServiceCategories,
                "Id",
                "Name");

            return View();
        }

        // POST CREATE (ADMIN ONLY)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(
                _context.ServiceCategories,
                "Id",
                "Name");

            return View(service);
        }
    }
}