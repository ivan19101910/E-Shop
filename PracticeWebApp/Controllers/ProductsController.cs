using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticeWebApp.Models;

namespace PracticeWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? id)
        {
            var products = _context.Products.Include(p => p.SubcategoryCategory).Select(x => x);
            if (id.HasValue)
            {
                products = products.Where(x => x.SubcategoryCategoryId == id);
            }
            //var appDbContext = _context.Products.Include(p => p.ProductSubcategory);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SubcategoryCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Comments"] =//HERE PROBLEM
                new List<Comment>(_context.Comments
                .Include(x => x.User)
                .ThenInclude(x=>x.UserRole)
                .Include(x => x.Product)
                .Include(x => x.RepliedComments)
                .ThenInclude(x=>x.RepliedComment)
                .Where(x => x.Product.Id == id));
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Адміністратор")]
        public IActionResult Create()
        {
            ViewData["ProductSubcategoryId"] = new SelectList(_context.SubcategoryCategories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Create([Bind("Id,Image,Price,Name,Description,ProductSubcategoryId")] Product product, IFormFile uploadImage)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)uploadImage.Length);
                }
                product.Image = imageData;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductSubcategoryId"] = new SelectList(_context.SubcategoryCategories, "Id", "Name", product.SubcategoryCategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductSubcategoryId"] = new SelectList(_context.SubcategoryCategories, "Id", "Name", product.SubcategoryCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Price,Name,Description,SubcategoryCategoryId")] Product product, IFormFile uploadImage)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)uploadImage.Length);
                    }
                    product.Image = imageData;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ProductSubcategoryId"] = new SelectList(_context.SubcategoryCategories, "Id", "Name", product.SubcategoryCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SubcategoryCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
