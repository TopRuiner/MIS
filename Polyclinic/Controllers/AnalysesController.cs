using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Areas.Identity.Data;
using Polyclinic.Data;
using Polyclinic.Models;
using System.Data;
using System.Security.Claims;

namespace Polyclinic.Controllers
{
    public class AnalysesController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly UserManager<PolyclinicUser> _userManager;

        public AnalysesController(UserManager<PolyclinicUser> userManager, PolyclinicContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Analyses
        //[Authorize(Roles = "Admin,Doctor,Assistant,Patient")]

        public async Task<IActionResult> Index()

        {
            var polyclinicContext = _context.Analyses.Include(a => a.Assistant).Include(a => a.Patient);

            if (HttpContext.User.IsInRole("Assistant"))
            {
                var user = _context.Assistants.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.AssistantId == user.Id);
            }
            else if (HttpContext.User.IsInRole("Patient"))
            {
                var user = _context.Patients.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.PatientId == user.Id);
            }

            return View(await polyclinicContext.ToListAsync());
        }
        [HttpGet]
        //[Authorize(Roles = "Admin,Doctor,Assistant,Patient")]

        public async Task<IActionResult> Index(string patientFIO, DateTime? patientBirthDate)
        {
            ViewData["PatientFIO"] = patientFIO;
            ViewData["PatientBirthDate"] = patientBirthDate;
            //var patientsQuery = from x in _context.Patients select x;
            var polyclinicContext = from x in _context.Analyses.Include(a => a.Assistant).Include(a => a.Patient) select x;
            if (HttpContext.User.IsInRole("Assistant"))
            {
                var user = _context.Assistants.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.AssistantId == user.Id);
            }
            else if (HttpContext.User.IsInRole("Patient"))
            {
                var user = _context.Patients.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.PatientId == user.Id);
            }
            //var polyclinicContext = _context.Analyses.Include(a => a.Assistant).Include(a => a.Patient);
            if (!String.IsNullOrEmpty(patientFIO))
            {
                string[] listPatientFIO = patientFIO.Split(' ');
                if (listPatientFIO.Length == 3)
                {
                    if (patientBirthDate != null)
                    {
                        polyclinicContext = polyclinicContext.Where(x => x.Patient.LastName.Contains(listPatientFIO[0]) && x.Patient.FirstName.Contains(listPatientFIO[1]) && x.Patient.MiddleName.Contains(listPatientFIO[2]) && x.Patient.BirthDate.Equals(patientBirthDate));
                    }
                    else
                    {
                        polyclinicContext = polyclinicContext.Where(x => x.Patient.LastName.Contains(listPatientFIO[0]) && x.Patient.FirstName.Contains(listPatientFIO[1]) && x.Patient.MiddleName.Contains(listPatientFIO[2]));
                    }
                }
            }
            return View(await polyclinicContext.AsNoTracking().ToListAsync());
        }




        // GET: Analyses/Details/5
        //[Authorize(Roles = "Admin,Doctor,Assistant")]

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
        //[Authorize(Roles = "Admin,Assistant")]

        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? assistantId = _context.Assistants.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["AssistantId"] = assistantId;

            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            return View();
        }

        // POST: Analyses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Assistant")]

        public async Task<IActionResult> Create([Bind("Id,PatientId,Type,Description,AssistantId")] Analysis analysis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analysis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? assistantId = _context.Assistants.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["AssistantId"] = assistantId;
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            return View(analysis);
        }

        // GET: Analyses/Edit/5
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
                return Problem("Entity set 'PolyclinicContext.Analyses'  is null.");
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
