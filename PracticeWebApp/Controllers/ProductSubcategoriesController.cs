using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticeWebApp.Models;

namespace PracticeWebApp.Controllers
{
    public class ProductSubcategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public ProductSubcategoriesController(AppDbContext context)
        {
            _context = context;
        }
        //Id for case when we need to get all subcategories in certain category
        // GET: ProductSubcategories
        public async Task<IActionResult> Index(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            var subcategories = _context.ProductSubcategories.Include(p => p.Category).Select(x=>x);
            if (id.HasValue)
            {
                subcategories = subcategories.Where(x => x.CategoryId == id);
            }

            //var appDbContext = _context.ProductSubcategory.Include(p => p.Category);
            
            
            return View(await subcategories.ToListAsync());
        }

        // GET: ProductSubcategories/Details/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSubcategory = await _context.ProductSubcategories
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            return View(productSubcategory);
        }

        // GET: ProductSubcategories/Create
        [Authorize(Roles = "Адміністратор")]
        public IActionResult Create()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name");
            //ViewData["Category"] = new _context.ProductCategories
            return View();
        }

        // POST: ProductSubcategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId")] ProductSubcategory productSubcategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSubcategory);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index(productSubcategory.CategoryId)));
                return Redirect($"~/ProductSubcategories/Index/{productSubcategory.CategoryId}");
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name", productSubcategory.CategoryId);
            return View(productSubcategory);
        }

        // GET: ProductSubcategories/Edit/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSubcategory = await _context.ProductSubcategories.FindAsync(id);
            if (productSubcategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name", productSubcategory.CategoryId);
            ViewData["Category"] = _context.ProductCategories.Where(x => x.Id == id).Select(x => x);
            return View(productSubcategory);
        }

        // POST: ProductSubcategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId")] ProductSubcategory productSubcategory)
        {
            if (id != productSubcategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSubcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSubcategoryExists(productSubcategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name", productSubcategory.CategoryId);
            return View(productSubcategory);
        }

        // GET: ProductSubcategories/Delete/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var productSubcategory = await _context.ProductSubcategories
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            return View(productSubcategory);
        }

        // POST: ProductSubcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSubcategory = await _context.ProductSubcategories.FindAsync(id);
            _context.ProductSubcategories.Remove(productSubcategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSubcategoryExists(int id)
        {
            return _context.ProductSubcategories.Any(e => e.Id == id);
        }
    }
}
