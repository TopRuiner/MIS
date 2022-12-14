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
    public class PatientsController : Controller
    {
        private readonly PolyclinicContext _context;

        public PatientsController(PolyclinicContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.Patients.Include(p => p.Polis).Include(p => p.PolyclinicUser);
            return View(await polyclinicContext.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.Polis)
                .Include(p => p.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewData["PolisID"] = new SelectList(_context.Polises, "Id", "Id");
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID,PolisID,SnilsNumber,WorkPlace")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                /*
                if (_context.Polises.FindAsync(patient.PolisID) != null && _context.Patients.Where(s => s.PolisID == patient.PolisID).FirstOrDefaultAsync() == null)
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    return Redirect("~/");
                }
                else
                {
                    ModelState.AddModelError("error_msg", "Введенный номер полиса уже занят или его не существует в системе");
                    ModelState.AddModelError("info_msg", "Для регистрации как пациент Вам необходимо сначала ввести данные своего полиса на страцнице регистрации пациента");
                    //return Redirect("~/123");
                    return View(patient);
                }
                */
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                return Redirect("~/");
            }
            ViewData["PolisID"] = new SelectList(_context.Polises, "Id", "Id", patient.PolisID);
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", patient.PolyclinicUserID);
            return Redirect("~/");
            //return View(patient);
        }

        // GET: Patients/Edit/5
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
            ViewData["PolisID"] = new SelectList(_context.Polises, "Id", "Id", patient.PolisID);
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", patient.PolyclinicUserID);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID,PolisID,SnilsNumber,WorkPlace")] Patient patient)
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
            ViewData["PolisID"] = new SelectList(_context.Polises, "Id", "Id", patient.PolisID);
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", patient.PolyclinicUserID);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.Polis)
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
