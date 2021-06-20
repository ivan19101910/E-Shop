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
    public class PostServicesController : Controller
    {
        private readonly AppDbContext _context;

        public PostServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PostServices
        public async Task<IActionResult> Index()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            return View(await _context.PostServices.ToListAsync());
        }

        // GET: PostServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var postService = await _context.PostServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postService == null)
            {
                return NotFound();
            }

            return View(postService);
        }

        // GET: PostServices/Create
        public IActionResult Create()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            return View();
        }

        // POST: PostServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PostService postService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postService);
        }

        // GET: PostServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var postService = await _context.PostServices.FindAsync(id);
            if (postService == null)
            {
                return NotFound();
            }
            return View(postService);
        }

        // POST: PostServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PostService postService)
        {
            if (id != postService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostServiceExists(postService.Id))
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
            return View(postService);
        }

        // GET: PostServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var postService = await _context.PostServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postService == null)
            {
                return NotFound();
            }

            return View(postService);
        }

        // POST: PostServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postService = await _context.PostServices.FindAsync(id);
            _context.PostServices.Remove(postService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostServiceExists(int id)
        {
            return _context.PostServices.Any(e => e.Id == id);
        }
    }
}
