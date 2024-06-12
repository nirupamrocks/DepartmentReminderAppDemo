using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DepartmentReminderAppDemo.Data;
using DepartmentReminderAppDemo.Models;

namespace DepartmentReminderApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments.Include(d => d.ParentDepartment).ToListAsync();
            return View(departments);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            ViewBag.ParentDepartments = _context.Departments.ToList();
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ParentDepartmentId")] Department department, IFormFile logoFile)
        {
            if (ModelState.IsValid)
            {
                if (logoFile != null && logoFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await logoFile.CopyToAsync(memoryStream);
                        department.Logo = memoryStream.ToArray();
                    }
                }

                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ParentDepartments = _context.Departments.ToList();
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            ViewBag.ParentDepartments = _context.Departments.ToList();
            return View(department);
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentDepartmentId")] Department department, IFormFile logoFile)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDepartment = await _context.Departments.FindAsync(id);
                    if (existingDepartment == null) return NotFound();

                    existingDepartment.Name = department.Name;
                    existingDepartment.ParentDepartmentId = department.ParentDepartmentId;
                    existingDepartment.UpdatedAt = DateTime.Now;

                    if (logoFile != null && logoFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await logoFile.CopyToAsync(memoryStream);
                            existingDepartment.Logo = memoryStream.ToArray();
                        }
                    }

                    _context.Update(existingDepartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ParentDepartments = _context.Departments.ToList();
            return View(department);
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments
                .Include(d => d.ParentDepartment)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null) return NotFound();

            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments
                .Include(d => d.ParentDepartment)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null) return NotFound();

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
