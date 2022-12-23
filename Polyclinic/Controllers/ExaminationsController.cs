using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Areas.Identity.Data;
using Polyclinic.Data;
using Polyclinic.Models;
using System.Security.Claims;

namespace Polyclinic.Controllers
{
    public class ExaminationsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly UserManager<PolyclinicUser> _userManager;
        public ExaminationsController(UserManager<PolyclinicUser> userManager, PolyclinicContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Examinations
        [Authorize(Roles = "Admin,Doctor,FunctionalDiagnosticsDoctor,Patient")]
        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.Examinations.Include(e => e.FunctionalDiagnosticsDoctor).Include(e => e.Patient);
            if (HttpContext.User.IsInRole("FunctionalDiagnosticsDoctor"))
            {
                var user = _context.FunctionalDiagnosticsDoctors.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.FunctionalDiagnosticsDoctorId == user.Id);
            }
            else if (HttpContext.User.IsInRole("Patient"))
            {
                var user = _context.Patients.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.PatientId == user.Id);
            }


            return View(await polyclinicContext.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,FunctionalDiagnosticsDoctor,Patient")]
        public async Task<IActionResult> Index(string patientFIO, DateTime? patientBirthDate)
        {
            ViewData["PatientFIO"] = patientFIO;
            ViewData["PatientBirthDate"] = patientBirthDate;
            //var patientsQuery = from x in _context.Patients select x;
            var polyclinicContext = from x in _context.Examinations.Include(a => a.FunctionalDiagnosticsDoctor).Include(a => a.Patient) select x;
            if (HttpContext.User.IsInRole("FunctionalDiagnosticsDoctor"))
            {
                var user = _context.FunctionalDiagnosticsDoctors.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.FunctionalDiagnosticsDoctorId == user.Id);
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

        // GET: Examinations/Details/5
        [Authorize(Roles = "Admin,Doctor,FunctionalDiagnosticsDoctor")]

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
        [Authorize(Roles = "FunctionalDiagnosticsDoctor,Admin")]

        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? functionalDiagnosticsDoctorId = _context.FunctionalDiagnosticsDoctors.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["FunctionalDiagnosticsDoctorId"] = functionalDiagnosticsDoctorId;
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            return View();
        }

        // POST: Examinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FunctionalDiagnosticsDoctor,Admin")]

        public async Task<IActionResult> Create([Bind("Id,Type,Description,FunctionalDiagnosticsDoctorId,PatientId")] Examination examination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? functionalDiagnosticsDoctorId = _context.FunctionalDiagnosticsDoctors.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["FunctionalDiagnosticsDoctorId"] = functionalDiagnosticsDoctorId;
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            return View(examination);
        }

        // GET: Examinations/Edit/5
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
                return Problem("Entity set 'PolyclinicContext.Examinations'  is null.");
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
