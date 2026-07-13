using HomeCare.Models;
using HomeCare.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST ALL CATEGORIES
        public IActionResult Index()
        {
            var categories = _context.ServiceCategories.ToList();

            return View(categories);
        }

        // GET CREATE PAGE
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.ServiceCategories.Add(category);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET EDIT PAGE
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.ServiceCategories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ServiceCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.ServiceCategories.Update(category);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET DELETE PAGE
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _context.ServiceCategories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.ServiceCategories.Find(id);

            if (category != null)
            {
                _context.ServiceCategories.Remove(category);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}