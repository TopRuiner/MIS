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
    public class PatientDoctorAppointmentsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly UserManager<PolyclinicUser> _userManager;

        public PatientDoctorAppointmentsController(UserManager<PolyclinicUser> userManager, PolyclinicContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null || _context.DoctorAppointments == null)
            {
                return NotFound();
            }

            var doctorAppointment = await _context.DoctorAppointments.FindAsync(id);
            if (doctorAppointment == null)
            {
                return NotFound();
            }
            doctorAppointment.Status = "Отменена пациентом";
            _context.Update(doctorAppointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PatientDoctorAppointments
        [Authorize(Roles = "Patient,Admin,Receptionist")]
        public async Task<IActionResult> Index()
        {
            var user = _context.Patients.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
            var polyclinicContext = _context.DoctorAppointments.Include(d => d.Doctor).Include(d => d.Patient).Where(p => p.PatientId == user.Id);

            return View(await polyclinicContext.ToListAsync());
        }
        [HttpGet]
        [Authorize(Roles = "Patient,Admin,Receptionist")]

        public async Task<IActionResult> Index(string option)
        {
            ViewData["Case"] = option;
            var user = _context.Patients.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();
            var doctorReferralQuery = from x in _context.DoctorAppointments.Include(d => d.Doctor).Include(d => d.Patient).Where(p => p.PatientId == user.Id) select x;
            if (!String.IsNullOrEmpty(option))
            {
                doctorReferralQuery = doctorReferralQuery.Where(x => x.Status.Contains(option));
            }
            return View(await doctorReferralQuery.AsNoTracking().ToListAsync());
        }

        // GET: PatientDoctorAppointments/Details/5
        [Authorize(Roles = "Receptionist,Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DoctorAppointments == null)
            {
                return NotFound();
            }

            var doctorAppointment = await _context.DoctorAppointments
                .Include(d => d.Doctor)
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorAppointment == null)
            {
                return NotFound();
            }

            return View(doctorAppointment);
        }

        // GET: PatientDoctorAppointments/Create
        [Authorize(Roles = "Patient,Admin,Receptionist")]

        public IActionResult Create()
        {
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "ReturnFIOAndSpeciality");
            //ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id");
            //ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "LastName");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? patientId = _context.Patients.Where(p => p.PolyclinicUserID == userId).FirstOrDefault().Id;
            ViewData["PatientId"] = patientId;
            /*
            List<Patient> patients = new List<Patient>
            {
                _context.Patients.Find(patientId)
            };
            ViewData["PatientId"] = new SelectList(patients, "Id", "Id");
            */
            return View();
        }

        // POST: PatientDoctorAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient,Admin,Receptionist")]

        public async Task<IActionResult> Create([Bind("Id,PatientId,CabinetId,DateTime,Status,DoctorId")] DoctorAppointment doctorAppointment)
        {
            if (ModelState.IsValid)
            {
                doctorAppointment.Status = "Ожидает подтверждения";
                _context.Add(doctorAppointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", doctorAppointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", doctorAppointment.PatientId);
            string messages = string.Join("; ", ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage));
            ModelState.AddModelError("error_msg", messages);
            return View(doctorAppointment);
        }

        // GET: PatientDoctorAppointments/Edit/5
        [Authorize(Roles = "Receptionist,Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DoctorAppointments == null)
            {
                return NotFound();
            }

            var doctorAppointment = await _context.DoctorAppointments.FindAsync(id);
            if (doctorAppointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", doctorAppointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", doctorAppointment.PatientId);
            return View(doctorAppointment);
        }

        // POST: PatientDoctorAppointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Receptionist,Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,CabinetId,DateTime,Status,DoctorId")] DoctorAppointment doctorAppointment)
        {
            if (id != doctorAppointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorAppointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorAppointmentExists(doctorAppointment.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", doctorAppointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", doctorAppointment.PatientId);
            return View(doctorAppointment);
        }

        // GET: PatientDoctorAppointments/Delete/5
        [Authorize(Roles = "Receptionist,Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DoctorAppointments == null)
            {
                return NotFound();
            }

            var doctorAppointment = await _context.DoctorAppointments
                .Include(d => d.Doctor)
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorAppointment == null)
            {
                return NotFound();
            }

            return View(doctorAppointment);
        }

        // POST: PatientDoctorAppointments/Delete/5
        [Authorize(Roles = "Receptionist,Admin")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DoctorAppointments == null)
            {
                return Problem("Entity set 'PolyclinicContext.DoctorAppointments'  is null.");
            }
            var doctorAppointment = await _context.DoctorAppointments.FindAsync(id);
            if (doctorAppointment != null)
            {
                _context.DoctorAppointments.Remove(doctorAppointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorAppointmentExists(int id)
        {
            return _context.DoctorAppointments.Any(e => e.Id == id);
        }
    }
}
