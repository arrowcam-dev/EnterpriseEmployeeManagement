using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseEmployeeManagement.Data;
using EnterpriseEmployeeManagement.Extensions;
using EnterpriseEmployeeManagement.Models;
using EnterpriseEmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeManagement.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DepartmentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // List Departments
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadDepartments(
            string search = "",
            string sortColumn = "name",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Departments
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.Name.Contains(search) ||
                    (e.Description != null && e.Description.Contains(search)));
            }

            // Sorting
            query = (sortColumn, sortDirection) switch
            {
                ("description", "asc") => query.OrderBy(e => e.Description),
                ("description", "desc") => query.OrderByDescending(e => e.Description),

                ("name", "desc") => query.OrderByDescending(e => e.Name),

                _ => query.OrderBy(e => e.Name)
            };

            var total = await query.CountAsync();

            var employees = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<DepartmentListViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var model = new PagedResult<DepartmentListViewModel>
            {
                Items = employees,
                TotalCount = total,
                PageNumber = page,
                PageSize = pageSize
            };

            return PartialView("_DepartmentTable", model);
        }

        public IActionResult CreateModal()
        {
            var vm = new DepartmentFormViewModel();

            return PartialView("_CreateDepartment", vm);
        }

        // POST Create
        [HttpPost]
        public async Task<IActionResult> CreateModal(DepartmentFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToErrorDictionary());
            }

            var department = _mapper.Map<Department>(vm);

            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> EditModal(Guid id)
        {
            var employee = await _context.Departments
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
                return NotFound();

            var vm = _mapper.Map<DepartmentFormViewModel>(employee);

            return PartialView("_EditDepartment", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditModal(DepartmentFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToErrorDictionary());
            }

            var department = await _context.Departments.FindAsync(vm.Id);

            if (department == null) return NotFound();

            _mapper.Map(vm, department);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<PartialViewResult> DetailsModal(Guid id)
        {
            var employee = await _context.Departments
                .FirstOrDefaultAsync(e => e.Id == id);

            return PartialView("_DetailsModal", employee);
        }

        // Delete
        public async Task<IActionResult> Delete(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }

            return Ok(department);
        }

        public async Task<IActionResult> Deleted()
        {
            var deletedDepartments = await _context.Departments
                .IgnoreQueryFilters()
                .Where(e => e.IsDeleted)
                .ToListAsync();

            return View(deletedDepartments);
        }

        public async Task<IActionResult> Restore(Guid id)
        {
            var employee = await _context.Departments
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
