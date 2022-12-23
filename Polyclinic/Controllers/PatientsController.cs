using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Areas.Identity.Data;
using Polyclinic.Data;
using Polyclinic.Models;
using System.Data;

namespace Polyclinic.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly SignInManager<PolyclinicUser> _signInManager;

        public PatientsController(SignInManager<PolyclinicUser> signInManager, PolyclinicContext context)
        {
            _context = context;
            _signInManager = signInManager;
        }


        // GET: Patients
        [Authorize(Roles = "Doctor,Admin,Receptionist")]
        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.Patients.Include(p => p.PolyclinicUser);

            return View(await polyclinicContext.ToListAsync());
        }
        [HttpGet]
        [Authorize(Roles = "Doctor,Admin,Receptionist")]
        public async Task<IActionResult> Index(string patientFIO, DateTime? patientBirthDate)
        {
            ViewData["PatientFIO"] = patientFIO;
            ViewData["PatientBirthDate"] = patientBirthDate;
            var patientsQuery = from x in _context.Patients select x;
            if (!String.IsNullOrEmpty(patientFIO))
            {
                string[] listPatientFIO = patientFIO.Split(' ');
                if (listPatientFIO.Length == 3)
                {
                    if (patientBirthDate != null)
                    {
                        patientsQuery = patientsQuery.Where(x => x.LastName.Contains(listPatientFIO[0]) && x.FirstName.Contains(listPatientFIO[1]) && x.MiddleName.Contains(listPatientFIO[2]) && x.BirthDate.Equals(patientBirthDate));
                    }
                    else
                    {
                        patientsQuery = patientsQuery.Where(x => x.LastName.Contains(listPatientFIO[0]) && x.FirstName.Contains(listPatientFIO[1]) && x.MiddleName.Contains(listPatientFIO[2]));
                    }
                }
            }
            return View(await patientsQuery.AsNoTracking().ToListAsync());
        }

        // GET: Patients/Details/5
        [Authorize(Roles = "Doctor,Admin,Receptionist")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create

        //[Authorize(Policy = "RequireCanRegisterAsPatient")]
        [Authorize(Roles = "CanRegisterAsPatient")]
        public IActionResult Create()
        {
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "RequireCanRegisterAsPatient")]
        [Authorize(Roles = "CanRegisterAsPatient")]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID,PolisID,PoilsCompany,PolisEndDate,SnilsNumber,WorkPlace")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                var userRoleBefore = new IdentityUserRole<string> { RoleId = "6", UserId = patient.PolyclinicUserID };
                var userRoleAfter = new IdentityUserRole<string> { RoleId = "7", UserId = patient.PolyclinicUserID };
                _context.UserRoles.Remove(userRoleBefore);
                _context.UserRoles.Add(userRoleAfter);

                await _context.SaveChangesAsync();
                PolyclinicUser user = _context.Users.Find(patient.PolyclinicUserID);


                //await _signInManager.SignOutAsync();
                //await _signInManager.SignInAsync(user, true);
                await _signInManager.RefreshSignInAsync(user);

                return Redirect("/");


                //return RedirectToAction(nameof(Index));
            }
            string messages = string.Join("; ", ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage));
            ModelState.AddModelError("error_msg", messages);

            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", patient.PolyclinicUserID);
            return View(patient);
        }

        // GET: Patients/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", patient.PolyclinicUserID);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID,PolisID,PoilsCompany,PolisEndDate,SnilsNumber,WorkPlace")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", patient.PolyclinicUserID);
            return View(patient);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patients == null)
            {
                return Problem("Entity set 'PolyclinicContext.Patients'  is null.");
            }
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
