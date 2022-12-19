using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polyclinic.Data;
using Polyclinic.Models;

namespace Polyclinic.Controllers
{
    public class ReceptionistsController : Controller
    {
        private readonly PolyclinicContext _context;

        public ReceptionistsController(PolyclinicContext context)
        {
            _context = context;
        }

        // GET: Receptionists
        public async Task<IActionResult> Index()
        {
            var polyclinicContext = _context.Receptionist.Include(r => r.PolyclinicUser);
            return View(await polyclinicContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index(string receptionistFIO, DateTime? receptionistBirthDate)
        {
            ViewData["ReceptionistFIO"] = receptionistFIO;
            ViewData["ReceptionistBirthDate"] = receptionistBirthDate;
            var receptionistsQuery = from x in _context.Receptionist select x;
            if (!String.IsNullOrEmpty(receptionistFIO))
            {
                string[] listReceptionistsQueryFIO = receptionistFIO.Split(' ');
                if (listReceptionistsQueryFIO.Length == 3)
                {
                    if (receptionistBirthDate != null)
                    {
                        receptionistsQuery = receptionistsQuery.Where(x => x.LastName.Contains(listReceptionistsQueryFIO[0]) && x.FirstName.Contains(listReceptionistsQueryFIO[1]) && x.MiddleName.Contains(listReceptionistsQueryFIO[2]) && x.BirthDate.Equals(receptionistBirthDate));
                    }
                    else
                    {
                        receptionistsQuery = receptionistsQuery.Where(x => x.LastName.Contains(listReceptionistsQueryFIO[0]) && x.FirstName.Contains(listReceptionistsQueryFIO[1]) && x.MiddleName.Contains(listReceptionistsQueryFIO[2]));
                    }
                }
            }
            return View(await receptionistsQuery.AsNoTracking().ToListAsync());
        }

        // GET: Receptionists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Receptionist == null)
            {
                return NotFound();
            }

            var receptionist = await _context.Receptionist
                .Include(r => r.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receptionist == null)
            {
                return NotFound();
            }

            return View(receptionist);
        }

        // GET: Receptionists/Create
        public IActionResult Create()
        {
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Receptionists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID")] Receptionist receptionist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receptionist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", receptionist.PolyclinicUserID);
            return View(receptionist);
        }

        // GET: Receptionists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Receptionist == null)
            {
                return NotFound();
            }

            var receptionist = await _context.Receptionist.FindAsync(id);
            if (receptionist == null)
            {
                return NotFound();
            }
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", receptionist.PolyclinicUserID);
            return View(receptionist);
        }

        // POST: Receptionists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleName,BirthDate,PolyclinicUserID")] Receptionist receptionist)
        {
            if (id != receptionist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receptionist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceptionistExists(receptionist.Id))
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
            ViewData["PolyclinicUserID"] = new SelectList(_context.Users, "Id", "Id", receptionist.PolyclinicUserID);
            return View(receptionist);
        }

        // GET: Receptionists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Receptionist == null)
            {
                return NotFound();
            }

            var receptionist = await _context.Receptionist
                .Include(r => r.PolyclinicUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receptionist == null)
            {
                return NotFound();
            }

            return View(receptionist);
        }

        // POST: Receptionists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Receptionist == null)
            {
                return Problem("Entity set 'PolyclinicContext.Receptionist'  is null.");
            }
            var receptionist = await _context.Receptionist.FindAsync(id);
            if (receptionist != null)
            {
                _context.Receptionist.Remove(receptionist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceptionistExists(int id)
        {
            return _context.Receptionist.Any(e => e.Id == id);
        }
    }
}
