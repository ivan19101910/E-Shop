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
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Index()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            var appDbContext = _context.Users.Include(u => u.UserRole);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Адміністратор")]
        public IActionResult Create()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Address,DateOfBirth,PhoneNumber,UserRoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "Id", "Id", user.UserRoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "Id", "Id", user.UserRoleId);
            ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "Id", "Name", user.UserRoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,DateOfBirth,PhoneNumber,UserRoleId,Password,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "Id", "Name", user.UserRoleId);
            return View(user);
        }


        [Authorize(Roles = "Адміністратор, Покупець")]
        public async Task<IActionResult> EditByEmail(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            var user = await _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefaultAsync();

            //var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["UserRoleId"] = new SelectList(_context.UserRoles.Where(x=>x.Id == user.UserRoleId), "Id", "Name", user.UserRoleId);
            //ViewData["UserRole"] = 
            //    _context.Users
            //    .Where(x => x.Email == User.Identity.Name)
            //    .Include(x=>x.UserRole)
            //    .Select(x => x)
            //    .FirstOrDefault().UserRole.Name;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор, Покупець")]
        public async Task<IActionResult> EditByEmail(int id, [Bind("Id,FirstName,LastName,Address,DateOfBirth,PhoneNumber,UserRoleId,Password,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return Redirect($"~/Home/Index");
            }
            ViewData["UserRoleId"] = new SelectList(_context.UserRoles, "Id", "Name", user.UserRoleId);
            return View(user);
        }


        // GET: Users/Delete/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
