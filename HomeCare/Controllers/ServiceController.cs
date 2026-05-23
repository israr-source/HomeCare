using HomeCare.Models;
using HomeCare.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST SERVICES
        public IActionResult Index()
        {
            var services = _context.Services
                .Include(s => s.ServiceCategory)
                .ToList();

            return View(services);
        }

        // GET CREATE PAGE
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                _context.ServiceCategories,
                "Id",
                "Name");

            return View();
        }

        // POST CREATE
        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(
                _context.ServiceCategories,
                "Id",
                "Name");

            return View(service);
        }
    }
}