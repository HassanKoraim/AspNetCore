using Microsoft.AspNetCore.Mvc;
using myshop.Web.Data;
using myshop.Web.Models;

namespace myshop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
               // category.CreatedTime = DateTime.Now;
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
           
            if (id == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                var categoryIndb = _context.Categories.Find(id);
                return View(categoryIndb);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                    _context.Categories.Update(category);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                var categoryIndb = _context.Categories.Find(id);
                return View(categoryIndb);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var category = _context.Categories.Find(id);
            if(category != null) 
            {
                Console.WriteLine("Hello");
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
                
            }
            return NotFound();
        }


        
    }
}
