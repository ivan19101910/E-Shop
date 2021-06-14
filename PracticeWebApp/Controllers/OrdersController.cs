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

        // GET: Orders
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Orders.Include(o => o.Status).Include(o => o.User);
            return View(await appDbContext.ToListAsync());
        }
        [Authorize(Roles = "Адміністратор")]
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Orders/Create
        //public IActionResult Create()
        //{
        //    ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id");
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
        //    return View();
        //}
        public IActionResult Create()
        {
            //var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            var cartProducts = _context.CartProducts.Where(x => x.UserId == user.Id).Include(x=>x.Product);
            decimal sum = 0;
            foreach(var cartProduct in cartProducts)
            {
                sum += cartProduct.Product.Price;
            }
            //ViewData["Products"] = new SelectList(_context.Products.Where();
            ViewData["Total"] = sum;
            ViewData["PostServiceId"] = new SelectList(_context.PostServices, "Id", "Name");
            //ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserId,Total,Description,StatusId")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", order.UserId);
        //    return View(order);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserId,Total,Description,StatusId")] Order order)
        //{
        //    var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
        //    var cartProducts = _context.CartProducts.Where(x => x.UserId == user.Id);
        //    if (ModelState.IsValid)
        //    {
        //        order.UserId = user.Id;
        //        order.Total = 1234;
        //        order.Description = "Desc";
        //        order.StatusId = 1;

        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        foreach (var cartProduct in cartProducts)
        //        {
        //            _context.Add(new OrderProduct { OrderId = order.Id, ProductId = cartProduct.ProductId, Amount = cartProduct.Amount });
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", order.UserId);
        //    return View(order);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,PostServiceId, City, PostDepartmentAddress")] Order order)
        {
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            var cartProducts = _context.CartProducts.Where(x => x.UserId == user.Id);
            if (ModelState.IsValid)
            {
                order.UserId = user.Id;
                order.Total = 1234;
                order.Description = "Desc";
                order.StatusId = 1;

                _context.Add(order);
                await _context.SaveChangesAsync();
                foreach (var cartProduct in cartProducts)
                {
                    _context.Add(new OrderProduct { OrderId = order.Id, ProductId = cartProduct.ProductId, Amount = cartProduct.Amount });
                }

                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Redirect("~/Home/Index");
            }
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", order.UserId);
            //return View(order);
            return View();
        }
        // GET: Orders/Edit/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
