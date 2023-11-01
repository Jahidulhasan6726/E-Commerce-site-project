using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using work_sessions.Models;

namespace work_sessions.Controllers
{
    public class SalesItemsController : Controller
    {
        private readonly ShopDbContext _context;

        public SalesItemsController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: SalesItems
        public async Task<IActionResult> Index()
        {
            var shopDbContext = _context.SalesItems.Include(s => s.Product).Include(s => s.SalesOrder);
            return View(await shopDbContext.ToListAsync());
        }

        // GET: SalesItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SalesItems == null)
            {
                return NotFound();
            }

            var salesItem = await _context.SalesItems
                .Include(s => s.Product)
                .Include(s => s.SalesOrder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesItem == null)
            {
                return NotFound();
            }

            return View(salesItem);
        }

        // GET: SalesItems/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders, "Id", "Id");
            return View();
        }

        // POST: SalesItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SalesOrderId,ProductId")] SalesItem salesItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", salesItem.ProductId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders, "Id", "Id", salesItem.SalesOrderId);
            return View(salesItem);
        }

        // GET: SalesItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SalesItems == null)
            {
                return NotFound();
            }

            var salesItem = await _context.SalesItems.FindAsync(id);
            if (salesItem == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", salesItem.ProductId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders, "Id", "Id", salesItem.SalesOrderId);
            return View(salesItem);
        }

        // POST: SalesItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SalesOrderId,ProductId")] SalesItem salesItem)
        {
            if (id != salesItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesItemExists(salesItem.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", salesItem.ProductId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders, "Id", "Id", salesItem.SalesOrderId);
            return View(salesItem);
        }

        // GET: SalesItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SalesItems == null)
            {
                return NotFound();
            }

            var salesItem = await _context.SalesItems
                .Include(s => s.Product)
                .Include(s => s.SalesOrder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesItem == null)
            {
                return NotFound();
            }

            return View(salesItem);
        }

        // POST: SalesItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SalesItems == null)
            {
                return Problem("Entity set 'ShopDbContext.SalesItems'  is null.");
            }
            var salesItem = await _context.SalesItems.FindAsync(id);
            if (salesItem != null)
            {
                _context.SalesItems.Remove(salesItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesItemExists(int id)
        {
          return (_context.SalesItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
