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
    public class ExaminationReferralsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly UserManager<PolyclinicUser> _userManager;

        public ExaminationReferralsController(UserManager<PolyclinicUser> userManager, PolyclinicContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ExaminationReferrals
        [Authorize(Roles = "Admin,Doctor,FunctionalDiagnosticsDoctor,Patient")]

        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.ExaminationReferrals.Include(e => e.Diagnosis).Include(e => e.Doctor).Include(e => e.FunctionalDiagnosticsDoctor).Include(e => e.Patient);
            if (HttpContext.User.IsInRole("Doctor"))
            {
                var user = _context.Doctors.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.DoctorId == user.Id);
            }
            else if (HttpContext.User.IsInRole("FunctionalDiagnosticsDoctor"))
            {
                var user = _context.Assistants.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.FunctionalDiagnosticsDoctorId == user.Id);
            }
            else if (HttpContext.User.IsInRole("Patient"))
            {
                var user = _context.Patients.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.PatientId == user.Id);
            }

            return View(await polyclinicContext.ToListAsync());
        }

        // GET: ExaminationReferrals/Details/5
        [Authorize(Roles = "Admin,Doctor,FunctionalDiagnosticsDoctor")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExaminationReferrals == null)
            {
                return NotFound();
            }

            var examinationReferral = await _context.ExaminationReferrals
                .Include(e => e.Diagnosis)
                .Include(e => e.Doctor)
                .Include(e => e.FunctionalDiagnosticsDoctor)
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examinationReferral == null)
            {
                return NotFound();
            }

            return View(examinationReferral);
        }

        // GET: ExaminationReferrals/Create
        [Authorize(Roles = "Admin,Doctor")]

        public IActionResult Create()
        {
            ViewBag.FunctionalDiagnosticsDoctors = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "ReturnFIOAndBirthDate");
            ViewBag.Diagnoses = new SelectList(_context.Diagnoses, "Id", "ReturnIdAndDescription");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? doctorId = _context.Doctors.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["DoctorId"] = doctorId;
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");


            /*
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id");
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id");
            ViewData["FunctionalDiagnosticsDoctorId"] = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "Id");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            */
            return View();
        }

        // POST: ExaminationReferrals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor")]

        public async Task<IActionResult> Create([Bind("Id,DiagnosisId,DoctorId,PatientId,Type,FunctionalDiagnosticsDoctorId,СabinetNum,DateTime")] ExaminationReferral examinationReferral)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examinationReferral);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id", examinationReferral.DiagnosisId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", examinationReferral.DoctorId);
            ViewData["FunctionalDiagnosticsDoctorId"] = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "Id", examinationReferral.FunctionalDiagnosticsDoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", examinationReferral.PatientId);
            return View(examinationReferral);
        }

        // GET: ExaminationReferrals/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExaminationReferrals == null)
            {
                return NotFound();
            }

            var examinationReferral = await _context.ExaminationReferrals.FindAsync(id);
            if (examinationReferral == null)
            {
                return NotFound();
            }
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id", examinationReferral.DiagnosisId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", examinationReferral.DoctorId);
            ViewData["FunctionalDiagnosticsDoctorId"] = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "Id", examinationReferral.FunctionalDiagnosticsDoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", examinationReferral.PatientId);
            return View(examinationReferral);
        }

        // POST: ExaminationReferrals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,DiagnosisId,DoctorId,PatientId,Type,FunctionalDiagnosticsDoctorId,СabinetNum,DateTime")] ExaminationReferral examinationReferral)
        {
            if (id != examinationReferral.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examinationReferral);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExaminationReferralExists(examinationReferral.Id))
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
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id", examinationReferral.DiagnosisId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", examinationReferral.DoctorId);
            ViewData["FunctionalDiagnosticsDoctorId"] = new SelectList(_context.FunctionalDiagnosticsDoctors, "Id", "Id", examinationReferral.FunctionalDiagnosticsDoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", examinationReferral.PatientId);
            return View(examinationReferral);
        }

        // GET: ExaminationReferrals/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExaminationReferrals == null)
            {
                return NotFound();
            }

            var examinationReferral = await _context.ExaminationReferrals
                .Include(e => e.Diagnosis)
                .Include(e => e.Doctor)
                .Include(e => e.FunctionalDiagnosticsDoctor)
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examinationReferral == null)
            {
                return NotFound();
            }

            return View(examinationReferral);
        }

        // POST: ExaminationReferrals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExaminationReferrals == null)
            {
                return Problem("Entity set 'PolyclinicContext.ExaminationReferrals'  is null.");
            }
            var examinationReferral = await _context.ExaminationReferrals.FindAsync(id);
            if (examinationReferral != null)
            {
                _context.ExaminationReferrals.Remove(examinationReferral);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExaminationReferralExists(int id)
        {
            return _context.ExaminationReferrals.Any(e => e.Id == id);
        }
    }
}
