using Microsoft.AspNetCore.Mvc;
using Polyclinic.Data;
using Polyclinic.Models;

namespace Polyclinic.Controllers
{
    public class RegisterPatientController : Controller
    {
        private readonly PolyclinicContext _context;

        public RegisterPatientController(PolyclinicContext context)
        {
            _context = context;
        }
        // GET: RegisterPatientController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RegisterPatientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RegisterPatientController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: RegisterPatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID,PolisID,SnilsNumber,WorkPlace")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                return Redirect("~/");
            }
            return View(patient);
        }

        // GET: RegisterPatientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RegisterPatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegisterPatientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RegisterPatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
