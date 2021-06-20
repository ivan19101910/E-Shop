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
    public class CartProductsController : Controller
    {
        private readonly AppDbContext _context;

        public CartProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CartProducts
        public async Task<IActionResult> Index()
        {
            ViewData["AllCategories"] = _context.GetAllCategories();
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();

            var appDbContext = _context.CartProducts.Include(c => c.Product).Include(c => c.User).Where(x=>x.UserId == user.Id);
            return View(await appDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Buy(int? id)
        {
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            
            var addedProduct = _context.Products.Where(x=>x.Id == id).Select(x => x).FirstOrDefault();
            var productsInCart = _context.CartProducts.Where(x => x.UserId == user.Id);
            //If a product already in a cart
            if (productsInCart.Any(x => x.ProductId == addedProduct.Id))
            {
                var product = _context.CartProducts.Where(x => x.ProductId == addedProduct.Id).FirstOrDefault();
                ++product.Amount;
                _context.Update(product);
            }
            else
            {
                _context.Add(new CartProduct { UserId = user.Id, ProductId = addedProduct.Id, Amount = 1 });
            }          
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return Redirect($"~/Products/Index/{addedProduct.SubcategoryCategoryId}");
            //return Ok();
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
            var cartProduct = _context.CartProducts.Where(x => x.ProductId == id && x.UserId == user.Id).FirstOrDefault();
            if (cartProduct.Amount > 1)
            {
                --cartProduct.Amount;
                _context.Update(cartProduct);
            }
            else
            {
                _context.CartProducts.Remove(cartProduct);                               
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            /*
            var cartProduct = await _context.CartProducts.FindAsync(id);
            _context.CartProducts.Remove(cartProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            */
        }

        // GET: CartProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartProduct = await _context.CartProducts
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (cartProduct == null)
            {
                return NotFound();
            }

            return View(cartProduct);
        }

        // GET: CartProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: CartProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,ProductId,Amount")] CartProduct cartProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", cartProduct.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", cartProduct.UserId);
            return View(cartProduct);
        }

        // GET: CartProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartProduct = await _context.CartProducts.FindAsync(id);
            if (cartProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", cartProduct.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", cartProduct.UserId);
            return View(cartProduct);
        }

        // POST: CartProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,ProductId,Amount")] CartProduct cartProduct)
        {
            if (id != cartProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartProductExists(cartProduct.ProductId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", cartProduct.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", cartProduct.UserId);
            return View(cartProduct);
        }

        // GET: CartProducts/Delete/5
        //Maybe need to check userId?
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cartProduct = await _context.CartProducts
        //        .Include(c => c.Product)
        //        .Include(c => c.User)
        //        .FirstOrDefaultAsync(m => m.ProductId == id);
        //    if (cartProduct == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cartProduct);
        //}

        //// POST: CartProducts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var cartProduct = await _context.CartProducts.FindAsync(id);
        //    _context.CartProducts.Remove(cartProduct);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CartProductExists(int id)
        {
            return _context.CartProducts.Any(e => e.ProductId == id);
        }
    }
}
