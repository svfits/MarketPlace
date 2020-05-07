using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoodsStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GoodsStore.Controllers
{
    [Authorize(Roles = "User")]
    public class ProductForSaleController : Controller
    {
        private readonly DataContextApp _context;
        private readonly UserManager<User> userManager;

        public ProductForSaleController(DataContextApp context, UserManager<User> _userManager)
        {
            _context = context;
            userManager = _userManager;
        }

        // GET: ProductForSale
        public async Task<IActionResult> Index(int? categories)
        {
            List<Product> model = null;

            if (categories == null)
            {
                ViewBag.Category = await _context.Category.ToListAsync();
                model = await _context.Product.Include(a => a.Images).Include(w => w.Category).ToListAsync();
                return base.View(model);
            }

            model = await _context.Product
                .Include(a => a.Images)
                .Include(w => w.Category)
                .Where(q => q.Category.Id == categories)
                .ToListAsync();

            ViewBag.Category = await _context.Category.ToListAsync();

            return base.View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            Basket basket = new Basket()
            {
                Product = _context.Product.First(a => a.Id == id),
                User = await userManager.GetUserAsync(HttpContext.User)
            };

            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
