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
    public class ExaminationsController : Controller
    {
        private readonly PolyclinicContext _context;

        public ExaminationsController(PolyclinicContext context)
        {
            _context = context;
        }

        // GET: Examinations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Examinations.Include(e => e.FunctionalDiagnosticsDoctor).Include(e => e.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Examinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Examinations == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations
                .Include(e => e.FunctionalDiagnosticsDoctor)
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examination == null)
            {
                return NotFound();
            }

            return View(examination);
        }

        // GET: Examinations/Create
        public IActionResult Create()
        {
            ViewData["FunctionalDiagnosticsDoctorId"] = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "Id");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            return View();
        }

        // POST: Examinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Description,FunctionalDiagnosticsDoctorId,PatientId")] Examination examination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FunctionalDiagnosticsDoctorId"] = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "Id", examination.FunctionalDiagnosticsDoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", examination.PatientId);
            return View(examination);
        }

        // GET: Examinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Examinations == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations.FindAsync(id);
            if (examination == null)
            {
                return NotFound();
            }
            ViewData["FunctionalDiagnosticsDoctorId"] = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "Id", examination.FunctionalDiagnosticsDoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", examination.PatientId);
            return View(examination);
        }

        // POST: Examinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Description,FunctionalDiagnosticsDoctorId,PatientId")] Examination examination)
        {
            if (id != examination.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExaminationExists(examination.Id))
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
            ViewData["FunctionalDiagnosticsDoctorId"] = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "Id", examination.FunctionalDiagnosticsDoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", examination.PatientId);
            return View(examination);
        }

        // GET: Examinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Examinations == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations
                .Include(e => e.FunctionalDiagnosticsDoctor)
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examination == null)
            {
                return NotFound();
            }

            return View(examination);
        }

        // POST: Examinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Examinations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Examinations'  is null.");
            }
            var examination = await _context.Examinations.FindAsync(id);
            if (examination != null)
            {
                _context.Examinations.Remove(examination);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExaminationExists(int id)
        {
          return _context.Examinations.Any(e => e.Id == id);
        }
    }
}
