using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticeWebApp.Models;

namespace PracticeWebApp.Controllers
{
    public class SubcategoryCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public SubcategoryCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SubcategoryCategories
        public async Task<IActionResult> Index(int ?id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            var appDbContext = _context.SubcategoryCategories.Include(s => s.ProductSubcategory).Select(x => x);
            if (id.HasValue)
            {
                appDbContext = appDbContext.Where(x => x.ProductSubcategoryId == id);
            }
            return View(await appDbContext.ToListAsync());
        }

        // GET: SubcategoryCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategoryCategory = await _context.SubcategoryCategories
                .Include(s => s.ProductSubcategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subcategoryCategory == null)
            {
                return NotFound();
            }

            return View(subcategoryCategory);
        }

        // GET: SubcategoryCategories/Create
        public IActionResult Create()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "Id", "Name");
            return View();
        }

        // POST: SubcategoryCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProductSubcategoryId")] SubcategoryCategory subcategoryCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subcategoryCategory);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Redirect($"~/SubcategoryCategories/Index/{subcategoryCategory.ProductSubcategoryId}");
            }
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "Id", "Name", subcategoryCategory.ProductSubcategoryId);
            return View(subcategoryCategory);
        }

        // GET: SubcategoryCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var subcategoryCategory = await _context.SubcategoryCategories.FindAsync(id);
            if (subcategoryCategory == null)
            {
                return NotFound();
            }
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "Id", "Name", subcategoryCategory.ProductSubcategoryId);
            return View(subcategoryCategory);
        }

        // POST: SubcategoryCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProductSubcategoryId")] SubcategoryCategory subcategoryCategory)
        {
            if (id != subcategoryCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subcategoryCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubcategoryCategoryExists(subcategoryCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return Redirect($"~/SubcategoryCategories/Index/{subcategoryCategory.ProductSubcategoryId}");
            }
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "Id", "Name", subcategoryCategory.ProductSubcategoryId);
            return View(subcategoryCategory);
        }

        // GET: SubcategoryCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var subcategoryCategory = await _context.SubcategoryCategories
                .Include(s => s.ProductSubcategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subcategoryCategory == null)
            {
                return NotFound();
            }

            return View(subcategoryCategory);
        }

        // POST: SubcategoryCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subcategoryCategory = await _context.SubcategoryCategories.FindAsync(id);
            _context.SubcategoryCategories.Remove(subcategoryCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubcategoryCategoryExists(int id)
        {
            return _context.SubcategoryCategories.Any(e => e.Id == id);
        }
    }
}
