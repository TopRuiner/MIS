using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Areas.Identity.Data;
using Polyclinic.Data;
using Polyclinic.Models;

namespace Polyclinic.Controllers
{
    public class DoctorDoctorAppointmentsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly UserManager<PolyclinicUser> _userManager;

        public DoctorDoctorAppointmentsController(UserManager<PolyclinicUser> userManager, PolyclinicContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DoctorDoctorAppointments
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var currentDoctorId = _context.Doctors.Where(d => d.PolyclinicUserID == currentUserId).FirstOrDefault();
            var polyclinicContext = _context.DoctorAppointments.Include(d => d.Doctor).Include(d => d.Patient).Where(d => d.DoctorId == currentDoctorId.Id);
            return View(await polyclinicContext.ToListAsync());
        }

        // GET: DoctorDoctorAppointments/Details/5
        [Authorize(Roles = "Doctor")]

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
        // POST: DoctorDoctorAppointments/Close/5
        [Authorize(Roles = "Doctor")]


        public async Task<IActionResult> Close(int? id)
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
            doctorAppointment.Status = "Использована";
            _context.Update(doctorAppointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: DoctorDoctorAppointments/Edit/5
        [Authorize(Roles = "Doctor")]
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
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "ReturnFIOAndSpeciality");
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            ViewData["Status"] = "Использована";
            return View(doctorAppointment);
        }

        // POST: DoctorDoctorAppointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]

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
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "ReturnFIOAndSpeciality");
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            ViewData["Status"] = "Использована";
            return View(doctorAppointment);
        }
        [HttpGet]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> Index(string? option, string? patientFIO, DateTime? patientBirthDate)
        {
            ViewData["Case"] = option;
            ViewData["PatientFIO"] = patientFIO;
            ViewData["PatientBirthDate"] = patientBirthDate;
            var doctorReferralQuery = from x in _context.DoctorAppointments.Include(d => d.Doctor).Include(d => d.Patient) select x;
            if (!String.IsNullOrEmpty(patientFIO))
            {
                string[] listPatientFIO = patientFIO.Split(' ');
                if (listPatientFIO.Length == 3)
                {
                    if (patientBirthDate != null)
                    {
                        doctorReferralQuery = doctorReferralQuery.Where(x => x.Patient.LastName.Contains(listPatientFIO[0]) && x.Patient.FirstName.Contains(listPatientFIO[1]) && x.Patient.MiddleName.Contains(listPatientFIO[2]) && x.Patient.BirthDate.Equals(patientBirthDate));
                    }
                    else
                    {
                        doctorReferralQuery = doctorReferralQuery.Where(x => x.Patient.LastName.Contains(listPatientFIO[0]) && x.Patient.FirstName.Contains(listPatientFIO[1]) && x.Patient.MiddleName.Contains(listPatientFIO[2]));
                    }
                }
            }
            if (!String.IsNullOrEmpty(option))
            {
                doctorReferralQuery = doctorReferralQuery.Where(x => x.Status.Contains(option));
            }
            return View(await doctorReferralQuery.AsNoTracking().ToListAsync());
        }

        private bool DoctorAppointmentExists(int id)
        {
            return _context.DoctorAppointments.Any(e => e.Id == id);
        }
    }
}
