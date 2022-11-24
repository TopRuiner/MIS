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
    public class AnalysesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalysesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Analyses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Analyses.Include(a => a.Assistant).Include(a => a.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Analyses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Analyses == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses
                .Include(a => a.Assistant)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return NotFound();
            }

            return View(analysis);
        }

        // GET: Analyses/Create
        public IActionResult Create()
        {
            ViewData["AssistantId"] = new SelectList(_context.Assistants, "Id", "Id");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            return View();
        }

        // POST: Analyses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,Type,Description,AssistantId")] Analysis analysis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analysis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssistantId"] = new SelectList(_context.Assistants, "Id", "Id", analysis.AssistantId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", analysis.PatientId);
            return View(analysis);
        }

        // GET: Analyses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Analyses == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses.FindAsync(id);
            if (analysis == null)
            {
                return NotFound();
            }
            ViewData["AssistantId"] = new SelectList(_context.Assistants, "Id", "Id", analysis.AssistantId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", analysis.PatientId);
            return View(analysis);
        }

        // POST: Analyses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,Type,Description,AssistantId")] Analysis analysis)
        {
            if (id != analysis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analysis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalysisExists(analysis.Id))
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
            ViewData["AssistantId"] = new SelectList(_context.Assistants, "Id", "Id", analysis.AssistantId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", analysis.PatientId);
            return View(analysis);
        }

        // GET: Analyses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Analyses == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses
                .Include(a => a.Assistant)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return NotFound();
            }

            return View(analysis);
        }

        // POST: Analyses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Analyses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Analyses'  is null.");
            }
            var analysis = await _context.Analyses.FindAsync(id);
            if (analysis != null)
            {
                _context.Analyses.Remove(analysis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalysisExists(int id)
        {
          return _context.Analyses.Any(e => e.Id == id);
        }
    }
}
