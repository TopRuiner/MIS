using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Data;
using Polyclinic.Models;

namespace Polyclinic.Controllers
{
    public class RegisterPolisController : Controller
    {
        private readonly PolyclinicContext _context;

        public RegisterPolisController(PolyclinicContext context)
        {
            _context = context;
        }
        // GET: RegisterPolisController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RegisterPolisController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RegisterPolisController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisterPolisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Polis.Id,Polis.Company,Polis.EndDate")] Polis polis)
        {
            if (ModelState.IsValid)
            {
                _context.Polises.Add(polis);
                await _context.SaveChangesAsync();
                return Redirect("~/Doctors");
            }
            return View(polis);
        }

        // GET: RegisterPolisController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RegisterPolisController/Edit/5
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

        // GET: RegisterPolisController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RegisterPolisController/Delete/5
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
