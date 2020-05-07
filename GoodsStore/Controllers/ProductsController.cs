using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoodsStore.Models;
using GoodsStore.ViewsModel;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace GoodsStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly DataContextApp _context;

        public ProductsController(DataContextApp context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.Include(a => a.Images).Include(w => w.Category).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(q => q.Category).Include(w => w.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Cataloges = _context.Category.AsEnumerable();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Cost,NameProduct,Category,Images")] ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var img = new List<Images>();

                if (productViewModel.Images != null)
                {
                    foreach (var item in productViewModel.Images)
                    {
                        // считываем переданный файл в массив байтов
                        using var binaryReader = new BinaryReader(item.OpenReadStream());
                        img.Add(new Images()
                        {
                            Body = Convert.ToBase64String(binaryReader.ReadBytes((int)item.Length)),
                            Name = item.FileName,
                        });
                    }
                }

                Product product = new Product()
                {
                    Cost = productViewModel.Cost,
                    NameProduct = productViewModel.NameProduct,
                    Images = img,
                    Description = productViewModel.Description,
                    Category = _context.Category.First(d => d.Id == productViewModel.Category.Id),
                };

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(u => u.Images)
                .Include(y => y.Category).FirstOrDefaultAsync(h => h.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Cataloges = _context.Category.AsEnumerable();

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Cost,NameProduct,Category")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     product.Category = _context.Category.FirstOrDefault(a => a.Id == product.Category.Id);
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
