using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Areas.Identity.Data;
using Polyclinic.Data;

namespace Polyclinic.Controllers
{
    public class PatientInspectionsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly UserManager<PolyclinicUser> _userManager;

        public PatientInspectionsController(UserManager<PolyclinicUser> userManager, PolyclinicContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        //Сделать INDEX для доктора и для пациента + Сортировка по дате

        // GET: Inspections
        [Authorize(Roles = "Patient")]

        public async Task<IActionResult> Index()
        {
            var user = _context.Patients.Where(u => u.PolyclinicUserID == _userManager.GetUserId(HttpContext.User)).FirstOrDefault();

            var polyclinicContext = _context.Inspections.Include(i => i.Diagnosis).Include(i => i.Doctor).Include(i => i.Patient).Where(p => p.PatientID == user.Id);
            return View(await polyclinicContext.ToListAsync());
        }

        // GET: Inspections/Details/5
        [Authorize(Roles = "Patient")]

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

        private bool InspectionExists(int id)
        {
            return _context.Inspections.Any(e => e.Id == id);
        }
    }
}
