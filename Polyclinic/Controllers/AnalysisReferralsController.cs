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
    public class AnalysisReferralsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly UserManager<PolyclinicUser> _userManager;

        public AnalysisReferralsController(UserManager<PolyclinicUser> userManager, PolyclinicContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AnalysisReferrals
        [Authorize(Roles = "Admin,Doctor,Assistant,Patient")]
        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.AnalysisReferrals.Include(a => a.Assistant).Include(a => a.Diagnosis).Include(a => a.Doctor).Include(a => a.Patient);
            if (HttpContext.User.IsInRole("Doctor"))
            {
                var user = _context.Doctors.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
                polyclinicContext.Where(r => r.DoctorId == user.Id);
            }
            else if (HttpContext.User.IsInRole("Assistant"))
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

        // GET: AnalysisReferrals/Details/5
        [Authorize(Roles = "Admin,Doctor,Assistant")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AnalysisReferrals == null)
            {
                return NotFound();
            }

            var analysisReferral = await _context.AnalysisReferrals
                .Include(a => a.Assistant)
                .Include(a => a.Diagnosis)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisReferral == null)
            {
                return NotFound();
            }

            return View(analysisReferral);
        }

        // GET: AnalysisReferrals/Create
        [Authorize(Roles = "Admin,Doctor")]

        public IActionResult Create()
        {
            ViewBag.Assistants = new SelectList(_context.Assistants, "Id", "ReturnFIOAndBirthDate");
            ViewBag.Diagnoses = new SelectList(_context.Diagnoses, "Id", "ReturnIdAndDescription");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? doctorId = _context.Doctors.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["DoctorId"] = doctorId;
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");

            /*
            ViewData["AssistantId"] = new SelectList(_context.Assistants, "Id", "Id");
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id");
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            */
            return View();
        }

        // POST: AnalysisReferrals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor")]

        public async Task<IActionResult> Create([Bind("Id,DiagnosisId,DoctorId,PatientId,Type,AssistantId,СabinetNum,DateTime")] AnalysisReferral analysisReferral)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analysisReferral);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Assistants = new SelectList(_context.Assistants, "Id", "ReturnFIOAndBirthDate");
            ViewBag.Diagnoses = new SelectList(_context.Diagnoses, "Id", "ReturnIdAndDescription");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? doctorId = _context.Doctors.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["DoctorId"] = doctorId;
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");


            //ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "ReturnFIOAndSpeciality");



            /*
            ViewData["AssistantId"] = new SelectList(_context.Assistants, "Id", "Id", analysisReferral.AssistantId);
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id", analysisReferral.DiagnosisId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", analysisReferral.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", analysisReferral.PatientId);
            */
            return View(analysisReferral);
        }

        // GET: AnalysisReferrals/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AnalysisReferrals == null)
            {
                return NotFound();
            }

            var analysisReferral = await _context.AnalysisReferrals.FindAsync(id);
            if (analysisReferral == null)
            {
                return NotFound();
            }
            ViewData["AssistantId"] = new SelectList(_context.Assistants, "Id", "Id", analysisReferral.AssistantId);
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id", analysisReferral.DiagnosisId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", analysisReferral.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", analysisReferral.PatientId);
            return View(analysisReferral);
        }

        // POST: AnalysisReferrals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,DiagnosisId,DoctorId,PatientId,Type,AssistantId,СabinetNum,DateTime")] AnalysisReferral analysisReferral)
        {
            if (id != analysisReferral.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analysisReferral);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalysisReferralExists(analysisReferral.Id))
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
            ViewData["AssistantId"] = new SelectList(_context.Assistants, "Id", "Id", analysisReferral.AssistantId);
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id", analysisReferral.DiagnosisId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", analysisReferral.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", analysisReferral.PatientId);
            return View(analysisReferral);
        }

        // GET: AnalysisReferrals/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AnalysisReferrals == null)
            {
                return NotFound();
            }

            var analysisReferral = await _context.AnalysisReferrals
                .Include(a => a.Assistant)
                .Include(a => a.Diagnosis)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisReferral == null)
            {
                return NotFound();
            }

            return View(analysisReferral);
        }

        // POST: AnalysisReferrals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AnalysisReferrals == null)
            {
                return Problem("Entity set 'PolyclinicContext.AnalysisReferrals'  is null.");
            }
            var analysisReferral = await _context.AnalysisReferrals.FindAsync(id);
            if (analysisReferral != null)
            {
                _context.AnalysisReferrals.Remove(analysisReferral);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalysisReferralExists(int id)
        {
            return _context.AnalysisReferrals.Any(e => e.Id == id);
        }
    }
}
