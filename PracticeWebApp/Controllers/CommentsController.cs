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
    public class CommentsController : Controller
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Comments.Include(c => c.Product).Include(c => c.User).Include(c=>c.RepliedComments);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public IActionResult Create(int? id)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["ProdId"] = id;
            return View();
        }
        [Authorize]
        public IActionResult CreateReply(int? id)
        {
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["ProdId"] = _context.Comments.Where(x=>x.Id == id).FirstOrDefault().ProductId;
            return View();
        }
        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Text")] Comment comment, int id)
        {
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            if (ModelState.IsValid)
            {
                comment.UserId = user.Id;
                comment.ProductId = id;
                comment.CreatedDateTime = DateTime.UtcNow;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Redirect($"~/Products/Details/{id}");
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", comment.UserId);
            return View(comment);
            //return View();
            //return Redirect($"~/Products/Details/{id}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReply([Bind("Text")] Comment comment, int id)//trouble here?
        {
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            //var repliedComment = _context.Products.Where(x=>x.Id == id).FirstOrDefault();
            var repliedComment = _context.Comments.Where(x => x.Id == id).FirstOrDefault();
            if (ModelState.IsValid)
            {               
                comment.UserId = user.Id;
                comment.RepliedCommentId = id;
                comment.ProductId = id;
                comment.CreatedDateTime = DateTime.UtcNow;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Redirect($"~/Products/Details/{repliedComment.ProductId}");
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", comment.UserId);
            ViewData["ProdId"] = comment.ProductId;
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,ProductId,UserId, CreatedDateTime, RepliedCommentId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", comment.UserId);
            ViewData["ProdId"] = comment.ProductId;
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewData["ProdId"] = comment.ProductId;
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
