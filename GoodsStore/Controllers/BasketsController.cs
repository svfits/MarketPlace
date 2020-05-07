using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodsStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GoodsStore.Controllers
{
    [Authorize(Roles = "User")]
    public class BasketsController : Controller
    {
        private readonly DataContextApp _context;
        private readonly UserManager<User> _userManager;

        public BasketsController(DataContextApp context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Baskets
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var baskets = await _context.Baskets
                .Include(a => a.Product)
                .Include(q => q.Product.Images)
                .Where(y => y.User == user)
                .ToListAsync();

            return View(baskets);
        }
    }
}
