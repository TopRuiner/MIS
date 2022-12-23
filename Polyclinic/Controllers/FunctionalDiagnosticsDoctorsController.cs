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
    public class FunctionalDiagnosticsDoctorsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly SignInManager<PolyclinicUser> _signInManager;

        public FunctionalDiagnosticsDoctorsController(SignInManager<PolyclinicUser> signInManager, PolyclinicContext context)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: FunctionalDiagnosticsDoctors
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.FunctionalDiagnosticsDoctors.Include(f => f.PolyclinicUser);
            return View(await polyclinicContext.ToListAsync());
        }

        // GET: FunctionalDiagnosticsDoctors/Details/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FunctionalDiagnosticsDoctors == null)
            {
                return NotFound();
            }

            var functionalDiagnosticsDoctor = await _context.FunctionalDiagnosticsDoctors
                .Include(f => f.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functionalDiagnosticsDoctor == null)
            {
                return NotFound();
            }

            return View(functionalDiagnosticsDoctor);
        }

        // GET: FunctionalDiagnosticsDoctors/Create
        [Authorize(Roles = "Admin,CanRegisterAsFunctionalDiagnosticsDoctor")]

        public IActionResult Create()
        {
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: FunctionalDiagnosticsDoctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanRegisterAsFunctionalDiagnosticsDoctor")]

        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID")] FunctionalDiagnosticsDoctor functionalDiagnosticsDoctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functionalDiagnosticsDoctor);
                var userRoleBefore = new IdentityUserRole<string> { RoleId = "9", UserId = functionalDiagnosticsDoctor.PolyclinicUserID };
                var userRoleAfter = new IdentityUserRole<string> { RoleId = "11", UserId = functionalDiagnosticsDoctor.PolyclinicUserID };
                _context.UserRoles.Remove(userRoleBefore);
                _context.UserRoles.Add(userRoleAfter);
                await _context.SaveChangesAsync();
                PolyclinicUser user = _context.Users.Find(functionalDiagnosticsDoctor.PolyclinicUserID);
                await _signInManager.RefreshSignInAsync(user);

                return Redirect("/");
            }
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", functionalDiagnosticsDoctor.PolyclinicUserID);
            return View(functionalDiagnosticsDoctor);
        }

        // GET: FunctionalDiagnosticsDoctors/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FunctionalDiagnosticsDoctors == null)
            {
                return NotFound();
            }

            var functionalDiagnosticsDoctor = await _context.FunctionalDiagnosticsDoctors.FindAsync(id);
            if (functionalDiagnosticsDoctor == null)
            {
                return NotFound();
            }
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", functionalDiagnosticsDoctor.PolyclinicUserID);
            return View(functionalDiagnosticsDoctor);
        }

        // POST: FunctionalDiagnosticsDoctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID")] FunctionalDiagnosticsDoctor functionalDiagnosticsDoctor)
        {
            if (id != functionalDiagnosticsDoctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functionalDiagnosticsDoctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionalDiagnosticsDoctorExists(functionalDiagnosticsDoctor.Id))
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
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", functionalDiagnosticsDoctor.PolyclinicUserID);
            return View(functionalDiagnosticsDoctor);
        }

        // GET: FunctionalDiagnosticsDoctors/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FunctionalDiagnosticsDoctors == null)
            {
                return NotFound();
            }

            var functionalDiagnosticsDoctor = await _context.FunctionalDiagnosticsDoctors
                .Include(f => f.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functionalDiagnosticsDoctor == null)
            {
                return NotFound();
            }

            return View(functionalDiagnosticsDoctor);
        }

        // POST: FunctionalDiagnosticsDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FunctionalDiagnosticsDoctors == null)
            {
                return Problem("Entity set 'PolyclinicContext.FunctionalDiagnosticsDoctors'  is null.");
            }
            var functionalDiagnosticsDoctor = await _context.FunctionalDiagnosticsDoctors.FindAsync(id);
            if (functionalDiagnosticsDoctor != null)
            {
                _context.FunctionalDiagnosticsDoctors.Remove(functionalDiagnosticsDoctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionalDiagnosticsDoctorExists(int id)
        {
            return _context.FunctionalDiagnosticsDoctors.Any(e => e.Id == id);
        }
    }
}
