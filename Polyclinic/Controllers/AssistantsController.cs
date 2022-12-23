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
    public class AssistantsController : Controller
    {
        private readonly PolyclinicContext _context;
        private readonly SignInManager<PolyclinicUser> _signInManager;

        public AssistantsController(SignInManager<PolyclinicUser> signInManager, PolyclinicContext context)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Assistants
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.Assistants.Include(a => a.PolyclinicUser);
            return View(await polyclinicContext.ToListAsync());
        }

        // GET: Assistants/Details/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Assistants == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistants
                .Include(a => a.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assistant == null)
            {
                return NotFound();
            }

            return View(assistant);
        }

        // GET: Assistants/Create
        [Authorize(Roles = "Admin,CanRegisterAsAssistant")]

        public IActionResult Create()
        {
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Assistants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanRegisterAsAssistant")]

        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID")] Assistant assistant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assistant);
                var userRoleBefore = new IdentityUserRole<string> { RoleId = "8", UserId = assistant.PolyclinicUserID };
                var userRoleAfter = new IdentityUserRole<string> { RoleId = "10", UserId = assistant.PolyclinicUserID };
                _context.UserRoles.Remove(userRoleBefore);
                _context.UserRoles.Add(userRoleAfter);
                await _context.SaveChangesAsync();
                PolyclinicUser user = _context.Users.Find(assistant.PolyclinicUserID);
                await _signInManager.RefreshSignInAsync(user);

                return Redirect("/");
            }
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", assistant.PolyclinicUserID);
            return View(assistant);
        }

        // GET: Assistants/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Assistants == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistants.FindAsync(id);
            if (assistant == null)
            {
                return NotFound();
            }
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", assistant.PolyclinicUserID);
            return View(assistant);
        }

        // POST: Assistants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID")] Assistant assistant)
        {
            if (id != assistant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assistant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssistantExists(assistant.Id))
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
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", assistant.PolyclinicUserID);
            return View(assistant);
        }

        // GET: Assistants/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Assistants == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistants
                .Include(a => a.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assistant == null)
            {
                return NotFound();
            }

            return View(assistant);
        }

        // POST: Assistants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assistants == null)
            {
                return Problem("Entity set 'PolyclinicContext.Assistants'  is null.");
            }
            var assistant = await _context.Assistants.FindAsync(id);
            if (assistant != null)
            {
                _context.Assistants.Remove(assistant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssistantExists(int id)
        {
            return _context.Assistants.Any(e => e.Id == id);
        }
    }
}
