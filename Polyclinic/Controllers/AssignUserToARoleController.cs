using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Polyclinic.Data;
using Polyclinic.Models;

namespace Polyclinic.Controllers
{
    public class AssignUserToARoleController : Controller
    {
        private readonly PolyclinicContext _context;

        public AssignUserToARoleController(PolyclinicContext context)
        {
            _context = context;
        }
        // GET: AssignUserToARoleController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AssignUserToARoleController/Create
        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: AssignUserToARoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("UserId,Role")] AssignUserToARoleModel assignUserToARole)
        {
            if (ModelState.IsValid)
            {
                var identityUserRole = new IdentityUserRole<string> { UserId = assignUserToARole.UserId, RoleId = assignUserToARole.Role };
                _context.UserRoles.Add(identityUserRole);
                await _context.SaveChangesAsync();
                return Redirect("/AssignUserToARole/Create");
            }
            ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name");
            string messages = string.Join("; ", ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage));
            ModelState.AddModelError("error_msg", messages);
            return View(assignUserToARole);
        }
    }
}
