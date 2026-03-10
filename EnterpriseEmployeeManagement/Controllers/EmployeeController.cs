using EnterpriseEmployeeManagement.Data;
using EnterpriseEmployeeManagement.Models;
using EnterpriseEmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Employee List
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();

            return View(employees);
        }

        // Deleted Employees (Admin)
        public async Task<IActionResult> Deleted()
        {
            var deletedEmployees = await _context.Employees
                .IgnoreQueryFilters()
                .Where(e => e.IsDeleted)
                .Include(e => e.Department)
                .ToListAsync();

            return View(deletedEmployees);
        }

        // Create Employee
        public IActionResult Create()
        {
            var vm = new EmployeeFormViewModel
            {
                HireDate = DateTime.Today,
                IsActive = true,
                Departments = _context.Departments
                 .Select(d => new SelectListItem
                 {
                     Value = d.Id.ToString(),
                     Text = d.Name
                 }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = _context.Departments
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList();

                return View(vm);
            }

            var employee = new Employee
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                DepartmentId = vm.DepartmentId,
                Position = vm.Position,
                HireDate = vm.HireDate,
                IsActive = vm.IsActive
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            var vm = new EmployeeFormViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                Position = employee.Position,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive,
                Departments = _context.Departments
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = _context.Departments
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList();

                return View(vm);
            }

            var employee = await _context.Employees.FindAsync(vm.Id);

            if (employee == null)
                return NotFound();

            employee.FirstName = vm.FirstName;
            employee.LastName = vm.LastName;
            employee.Email = vm.Email;
            employee.DepartmentId = vm.DepartmentId;
            employee.Position = vm.Position;
            employee.HireDate = vm.HireDate;
            employee.IsActive = vm.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Soft Delete
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Restore Deleted Employee
        public async Task<IActionResult> Restore(int id)
        {
            var employee = await _context.Employees
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                employee.IsDeleted = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Deleted));
        }

    }
}
