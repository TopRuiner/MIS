using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Data;
using Polyclinic.Models;

namespace Polyclinic.Controllers
{
    public class DiagnosesController : Controller
    {
        private readonly PolyclinicContext _context;

        public DiagnosesController(PolyclinicContext context)
        {
            _context = context;
        }

        // GET: Diagnoses
        public async Task<IActionResult> Index()
        {
              return View(await _context.Diagnoses.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index(string DiagnosisSearch)
        {
            ViewData["GetDiagnosisDetails"] = DiagnosisSearch;
            var diagnosisQuerry = from x in _context.Diagnoses select(x);
            if (!String.IsNullOrEmpty(DiagnosisSearch))
            {
                diagnosisQuerry = diagnosisQuerry.Where(x => x.Description.Contains(DiagnosisSearch) || x.MKB.Contains(DiagnosisSearch));
            }
            return View(await diagnosisQuerry.AsNoTracking().ToListAsync());
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // GET: Diagnoses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MKB,Description")] Diagnosis diagnosis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diagnosis);
        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
            {
                return NotFound();
            }
            return View(diagnosis);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MKB,Description")] Diagnosis diagnosis)
        {
            if (id != diagnosis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisExists(diagnosis.Id))
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
            return View(diagnosis);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Diagnoses == null)
            {
                return Problem("Entity set 'PolyclinicContext.Diagnoses'  is null.");
            }
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis != null)
            {
                _context.Diagnoses.Remove(diagnosis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisExists(int id)
        {
          return _context.Diagnoses.Any(e => e.Id == id);
        }
    }
}
