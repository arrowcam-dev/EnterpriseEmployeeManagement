using AutoMapper;
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
        private readonly IMapper _mapper;
        public EmployeeController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            
            var employee = _mapper.Map<Employee>(vm);

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            var vm = _mapper.Map<EmployeeFormViewModel>(employee);

            vm.Departments = _context.Departments
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList();

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

            _mapper.Map(vm, employee);

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
