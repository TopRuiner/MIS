using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Data;
using Polyclinic.Models;
using System.Data;

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
        [Authorize(Roles = "Admin,Doctor")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.Diagnoses.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index(string diagnosisSearch)
        {
            ViewData["GetDiagnosisDetails"] = diagnosisSearch;
            var diagnosisQuery = from x in _context.Diagnoses select x;
            if (!String.IsNullOrEmpty(diagnosisSearch))
            {
                diagnosisQuery = diagnosisQuery.Where(x => x.Id.Contains(diagnosisSearch) || x.Description.Contains(diagnosisSearch));
            }
            return View(await diagnosisQuery.AsNoTracking().ToListAsync());
        }

        // GET: Diagnoses/Details/5
        [Authorize(Roles = "Admin,Doctor")]

        public async Task<IActionResult> Details(string id)
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
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] Diagnosis diagnosis)
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(string id)
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(string id, [Bind("Id,Description")] Diagnosis diagnosis)
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(string id)
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(string id)
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

        private bool DiagnosisExists(string id)
        {
            return _context.Diagnoses.Any(e => e.Id == id);
        }
    }
}
