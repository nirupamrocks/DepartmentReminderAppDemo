using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DepartmentReminderAppDemo.Models;
using DepartmentReminderAppDemo.Data;


namespace DepartmentReminderAppDemo.Controllers
{
    public class ReminderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReminderController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Reminder
        public async Task<IActionResult> Index()
        {
            var reminders = await _context.Reminders.Include(r => r.Department).ToListAsync();
            return View(reminders);
        }

        // GET: Reminder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reminder = await _context.Reminders
                .Include(r => r.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reminder == null) return NotFound();

            return View(reminder);
        }

        // GET: Reminder/Create
        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }

        // POST: Reminder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,DueDate,Description,DepartmentId")] Reminder reminder)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(reminder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = _context.Departments.ToList();
            return View(reminder);
        }

        // GET: Reminder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null) return NotFound();

            ViewBag.Departments = _context.Departments.ToList();
            return View(reminder);
        }

        // POST: Reminder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DueDate,Description,DepartmentId,IsNotified")] Reminder reminder)
        {
            if (id != reminder.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(reminder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReminderExists(reminder.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = _context.Departments.ToList();
            return View(reminder);
        }

        // GET: Reminder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reminder = await _context.Reminders
                .Include(r => r.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reminder == null) return NotFound();

            return View(reminder);
        }

        // POST: Reminder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReminderExists(int id)
        {
            return _context.Reminders.Any(e => e.Id == id);
        }

    }
}
