using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlueSkySystem.Data;
using BlueSkySystem.Models;
using System.Security.Claims;

namespace BlueSkySystem.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employees.Include(e => e.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EmpNo == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,EmpNo,FirstName,MiddleName,LastName,Department,Designation,Address,BirthDate,CreatedById,CreatedOn,ModifiedById,ModifiedOn")] Employee employee)
        {
            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the current user from the database
            var currentUser = await _context.Users.FindAsync(userId);

            // Replace userId with the user's name (FirstName or FullName)
            employee.CreatedById = currentUser.FullName;  // You can also use currentUser.FirstName if that's what you prefer
            employee.CreatedOn = DateTime.Now;

            if (ModelState.IsValid)
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", employee.UserId);
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }


        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", employee.UserId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,EmpNo,FirstName,MiddleName,LastName,Department,Designation,Address,BirthDate,ModifiedById,ModifiedOn")] Employee employee)
        {
            if (id != employee.EmpNo)
            {
                return NotFound();
            }

            // Retrieve the current employee from the database
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the current user from the database
            var currentUser = await _context.Users.FindAsync(userId);

            // Keep the existing CreatedById and update the ModifiedById
            employee.CreatedById = existingEmployee.CreatedById;  // Keep the original CreatedById
            employee.CreatedOn = existingEmployee.CreatedOn;  // Keep the original CreatedOn
            employee.ModifiedById = currentUser.FullName;  // Update ModifiedById with the current user's name
            employee.ModifiedOn = DateTime.Now;  // Set the current date and time

            // Update the employee in the database
            try
            {
                _context.Entry(existingEmployee).CurrentValues.SetValues(employee);  // Update fields selectively
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Redirect to Index after successful save
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.EmpNo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Repopulate ViewData for the dropdown
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", employee.UserId);
            return View(employee);
        }




        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EmpNo == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmpNo == id);
        }
    }
}
