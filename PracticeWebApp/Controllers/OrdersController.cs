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
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Адміністратор, Покупець")]
        public async Task<IActionResult> Index()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            var appDbContext = _context.Orders.Include(o => o.Status).Include(o => o.User).Where(x => x.UserId == user.Id);
            return View(await appDbContext.ToListAsync());
        }
        // GET: Orders
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> IndexAll()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            var appDbContext = _context.Orders.Include(o => o.Status).Include(o => o.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }
            
            var order = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.User)
                .Include(o=>o.PostService)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            ViewData["OrderProducts"] = 
                new List<OrderProduct>(_context.OrderProducts
                .Include(x => x.Order)
                .Include(x=>x.Product)
                .Where(x => x.Order.UserId == order.UserId && x.OrderId == order.Id));
            return View(order);
        }

        [Authorize(Roles = "Адміністратор, Покупець")]
        public IActionResult Create()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            var cartProducts = _context.CartProducts.Where(x => x.UserId == user.Id).Include(x=>x.Product);
            decimal sum = 0;
            foreach(var cartProduct in cartProducts)
            {
                sum += cartProduct.Product.Price * cartProduct.Amount;
            }
           
            ViewData["Total"] = sum;
            ViewData["PostServiceId"] = new SelectList(_context.PostServices, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор, Покупець")]
        public async Task<IActionResult> Create([Bind("Id,Total,PostServiceId, City, PostDepartmentAddress")] Order order)
        {
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            var cartProducts = _context.CartProducts.Where(x => x.UserId == user.Id);
            if (ModelState.IsValid)
            {
                order.UserId = user.Id;
                //order.Total = 1234;
                order.Description = "Desc";
                order.StatusId = 1;
                order.CreatedDateTime = DateTime.UtcNow;

                _context.Add(order);
                await _context.SaveChangesAsync();
                foreach (var cartProduct in cartProducts)
                {
                    _context.Add(new OrderProduct { OrderId = order.Id, ProductId = cartProduct.ProductId, Amount = cartProduct.Amount });
                }

                
                foreach (var el in cartProducts)
                {
                    _context.CartProducts.Remove(el);
                }
                await _context.SaveChangesAsync();
                return Redirect("~/Home/Index");
            }
            
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", order.UserId);
            return View();
        }
        // GET: Orders/Edit/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Name", order.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Total,Description,StatusId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
