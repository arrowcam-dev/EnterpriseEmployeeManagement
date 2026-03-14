using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseEmployeeManagement.Data;
using EnterpriseEmployeeManagement.Extensions;
using EnterpriseEmployeeManagement.Models;
using EnterpriseEmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeManagement.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public EmployeesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
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


        // Soft Delete
        public async Task<IActionResult> Delete(Guid id)
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
        public async Task<IActionResult> Restore(Guid id)
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

        [HttpGet]
        public async Task<IActionResult> LoadEmployees(
            string search = "",
            string sortColumn = "name",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search) ||
                    e.Email.Contains(search));
            }

            // Sorting
            query = (sortColumn, sortDirection) switch
            {
                ("email", "asc") => query.OrderBy(e => e.Email),
                ("email", "desc") => query.OrderByDescending(e => e.Email),

                ("department", "asc") => query.OrderBy(e => e.Department.Name),
                ("department", "desc") => query.OrderByDescending(e => e.Department.Name),

                ("name", "desc") => query.OrderByDescending(e => e.FirstName),

                _ => query.OrderBy(e => e.FirstName)
            };

            var total = await query.CountAsync();

            var employees = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<EmployeeListViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var model = new PagedResult<EmployeeListViewModel>
            {
                Items = employees,
                TotalCount = total,
                PageNumber = page,
                PageSize = pageSize
            };

            return PartialView("_EmployeeTable", model);
        }

        public IActionResult CreateModal()
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

            return PartialView("Partials/_CreateEmployeeForm", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModal(EmployeeFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToErrorDictionary());
            }

            var employee = _mapper.Map<Employee>(vm);

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> EditModal(Guid id)
        {
            var employee = await _context.Employees
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
                return NotFound();

            var vm = _mapper.Map<EmployeeFormViewModel>(employee);

            vm.Departments = _context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

            return PartialView("Partials/_EditEmployeeForm", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditModal(EmployeeFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToErrorDictionary());
            }

            var employee = await _context.Employees.FindAsync(vm.Id);

            if (employee == null) return NotFound();

            _mapper.Map(vm, employee);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<PartialViewResult> DetailsModal(Guid id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            return PartialView("_DetailsModal", employee);
        }

    }
}
