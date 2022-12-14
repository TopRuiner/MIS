using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Data;
using Polyclinic.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Polyclinic.Controllers
{
    public class PolisController : Controller
    {
        private readonly PolyclinicContext _context;

        public PolisController(PolyclinicContext context)
        {
            _context = context;
        }

        // GET: Polis
        public async Task<IActionResult> Index()
        {
              return View(await _context.Polises.ToListAsync());
        }

        // GET: Polis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Polises == null)
            {
                return NotFound();
            }

            var polis = await _context.Polises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (polis == null)
            {
                return NotFound();
            }

            return View(polis);
        }

        // GET: Polis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Polis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Company,EndDate")] Polis polis)
        {
            if (ModelState.IsValid)
            {
                if (_context.Polises.Find(polis.Id) == null)
                {
                    _context.Add(polis);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return Redirect("~/");
                }
                else
                {
                    ModelState.AddModelError("error_msg", "Введенный номер полиса уже существует в системе");
                    return View(polis);
                }
            }
            return View(polis);
        }

        // GET: Polis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Polises == null)
            {
                return NotFound();
            }

            var polis = await _context.Polises.FindAsync(id);
            if (polis == null)
            {
                return NotFound();
            }
            return View(polis);
        }

        // POST: Polis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Company,EndDate")] Polis polis)
        {
            if (id != polis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(polis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolisExists(polis.Id))
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
            return View(polis);
        }

        // GET: Polis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Polises == null)
            {
                return NotFound();
            }

            var polis = await _context.Polises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (polis == null)
            {
                return NotFound();
            }

            return View(polis);
        }

        // POST: Polis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Polises == null)
            {
                return Problem("Entity set 'PolyclinicContext.Polises'  is null.");
            }
            var polis = await _context.Polises.FindAsync(id);
            if (polis != null)
            {
                _context.Polises.Remove(polis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PolisExists(int id)
        {
          return _context.Polises.Any(e => e.Id == id);
        }
    }
}
