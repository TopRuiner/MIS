using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Data;
using Polyclinic.Models;

namespace Polyclinic.Controllers
{
    public class ReceptionistDoctorAppointmentsController : Controller
    {
        private readonly PolyclinicContext _context;

        public ReceptionistDoctorAppointmentsController(PolyclinicContext context)
        {
            _context = context;
        }

        // GET: DoctorAppointments
        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.DoctorAppointments.Include(d => d.Doctor).Include(d => d.Patient);
            return View(await polyclinicContext.ToListAsync());
        }

        // GET: DoctorAppointments/Details/5
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
        // POST: DoctorAppointments/Confirm/5

        public async Task<IActionResult> Decline(int? id)
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
            doctorAppointment.Status = "Отменена регистраутрой";
            _context.Update(doctorAppointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: DoctorAppointments/Create
        public IActionResult Create()
        {
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "ReturnFIOAndSpeciality");
            //ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id");
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            //ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            ViewData["Status"] = "Подтвержден";
            return View();
        }

        // POST: DoctorAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,CabinetId,DateTime,Status,DoctorId")] DoctorAppointment doctorAppointment)
        {
            if (ModelState.IsValid)
            {
                doctorAppointment.Status = "Подтверждена";
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

        // GET: DoctorAppointments/Edit/5
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
            //ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", doctorAppointment.DoctorId);
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "ReturnFIOAndSpeciality");
            //ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", doctorAppointment.PatientId);
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "ReturnFIOAndBirthDate");
            ViewData["Status"] = "Подтверждена";
            return View(doctorAppointment);
        }

        // POST: DoctorAppointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    HttpClient client = new HttpClient();
                    var patient = _context.Patients.Find(doctorAppointment.PatientId);
                    var doctor = _context.Doctors.Find(doctorAppointment.DoctorId);
                    var patientUser = _context.Users.Find(patient.PolyclinicUserID);
                    string email = patientUser.Email;
                    string message = "Ваша заявка на прием ко врачу подтверждена регистратурой\nДанные направления:" + "\nКабинет:" + doctorAppointment.CabinetId + "\nВремя:" + doctorAppointment.DateTime + "\nДоктор:" + doctor.ReturnFIOAndSpeciality;
                    string url = string.Format("https://localhost:7262/Approved/SendEmailNotification?email={0}&message={1}", email, message);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var requestResult = new JsonResult(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        var requestResult = "Ошибка";
                    }
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

        // GET: DoctorAppointments/Delete/5
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

        // POST: DoctorAppointments/Delete/5
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
        [HttpGet]
        public async Task<IActionResult> Index(string option)
        {
            ViewData["Case"] = option;
            var doctorReferralQuery = from x in _context.DoctorAppointments.Include(d => d.Doctor).Include(d => d.Patient) select x;
            if (!String.IsNullOrEmpty(option))
            {
                doctorReferralQuery = doctorReferralQuery.Where(x => x.Status.Contains(option));
            }
            /*
            switch (option)
            {
                case "1":
                    doctorReferralQuery = doctorReferralQuery.Where(x => x.Status.Contains("Ожидает подтверждения"));
                    break;
                case "2":
                    doctorReferralQuery = doctorReferralQuery.Where(x => x.Status.Contains("Подтверждена"));
                    break;
                case "3":
                    doctorReferralQuery = doctorReferralQuery.Where(x => x.Status.Contains("Отменена регистраутрой"));
                    break;
                case "4":
                    doctorReferralQuery = doctorReferralQuery.Where(x => x.Status.Contains("Отменена пациентом"));
                    break;
            }
            */
            return View(await doctorReferralQuery.AsNoTracking().ToListAsync());
        }

        private bool DoctorAppointmentExists(int id)
        {
            return _context.DoctorAppointments.Any(e => e.Id == id);
        }
    }
}
