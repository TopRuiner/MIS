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
    public class DoctorInspectionsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly UserManager<PolyclinicUser> _userManager;

        public DoctorInspectionsController(UserManager<PolyclinicUser> userManager, PolyclinicContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        //Сделать INDEX для доктора и для пациента + Сортировка по дате

        // GET: Inspections
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> Index()
        {
            var user = _context.Doctors.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();

            var polyclinicContext = _context.Inspections.Include(i => i.Diagnosis).Include(i => i.Doctor).Include(i => i.Patient).Where(p => p.DoctorId == user.Id);
            return View(await polyclinicContext.ToListAsync());
        }

        // GET: Inspections/Details/5
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inspections == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspections
                .Include(i => i.Diagnosis)
                .Include(i => i.Doctor)
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inspection == null)
            {
                return NotFound();
            }

            return View(inspection);
        }

        // GET: Inspections/Create
        [Authorize(Roles = "Doctor")]
        public IActionResult Create()
        {
            ViewBag.Diagnoses = new SelectList(_context.Diagnoses, "Id", "ReturnIdAndDescription");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? doctorId = _context.Doctors.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["DoctorId"] = doctorId;

            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");

            /*
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id");
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id");
            ViewData["PatientID"] = new SelectList(_context.Patients, "Id", "Id");
            */
            return View();
        }

        // POST: Inspections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> Create([Bind("Id,PatientID,Complaint,Recipe,DiagnosisId,Date,Type,DoctorId")] Inspection inspection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inspection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Diagnoses = new SelectList(_context.Diagnoses, "Id", "ReturnIdAndDescription");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? doctorId = _context.Doctors.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["DoctorId"] = doctorId;

            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            return View(inspection);
        }

        // GET: Inspections/Edit/5
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inspections == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id", inspection.DiagnosisId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", inspection.DoctorId);
            ViewData["PatientID"] = new SelectList(_context.Patients, "Id", "Id", inspection.PatientID);
            return View(inspection);
        }

        // POST: Inspections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientID,Complaint,Recipe,DiagnosisId,Date,Type,DoctorId")] Inspection inspection)
        {
            if (id != inspection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inspection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InspectionExists(inspection.Id))
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
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "Id", "Id", inspection.DiagnosisId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", inspection.DoctorId);
            ViewData["PatientID"] = new SelectList(_context.Patients, "Id", "Id", inspection.PatientID);
            return View(inspection);
        }

        // GET: Inspections/Delete/5
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inspections == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspections
                .Include(i => i.Diagnosis)
                .Include(i => i.Doctor)
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inspection == null)
            {
                return NotFound();
            }

            return View(inspection);
        }

        // POST: Inspections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inspections == null)
            {
                return Problem("Entity set 'PolyclinicContext.Inspections'  is null.");
            }
            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection != null)
            {
                _context.Inspections.Remove(inspection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InspectionExists(int id)
        {
            return _context.Inspections.Any(e => e.Id == id);
        }
    }
}
