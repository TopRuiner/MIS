using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Polyclinic.Controllers
{
    public class RegisterPatientController : Controller
    {
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
        public ActionResult Create(IFormCollection collection)
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
