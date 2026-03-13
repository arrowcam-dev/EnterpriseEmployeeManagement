using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseEmployeeManagement.Constants;
using EnterpriseEmployeeManagement.Data;
using EnterpriseEmployeeManagement.Extensions;
using EnterpriseEmployeeManagement.Helpers;
using EnterpriseEmployeeManagement.Models;
using EnterpriseEmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeManagement.Controllers
{
    [Authorize(Roles = DefaultRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Deleted()
        {
            var deletedUsers = await _context.Users
                .IgnoreQueryFilters()
                .Where(e => e.IsDeleted)
                .Include(e => e.Roles)
                .ToListAsync();

            return View(deletedUsers);
        }

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

        public async Task<PartialViewResult> LoadUsers(
            string search = "",
            string sortColumn = "username",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Users
                .Include(x => x.Roles).ThenInclude(x => x.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Username.Contains(search) ||
                    x.Email.Contains(search));
            }

            // Sorting
            query = (sortColumn, sortDirection) switch
            {
                ("email", "asc") => query.OrderBy(e => e.Email),
                ("email", "desc") => query.OrderByDescending(e => e.Email),

                ("username", "desc") => query.OrderByDescending(e => e.Username),

                _ => query.OrderBy(e => e.Username)
            };

            var total = await query.CountAsync();

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<UserListViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var model = new PagedResult<UserListViewModel>
            {
                Items = users,
                TotalCount = total,
                PageNumber = page,
                PageSize = pageSize
            };

            return PartialView("_UsersTable", model);
        }

        public IActionResult CreateModal()
        {
            var vm = new UserEditViewModel
            {
                AvailableRoles = DefaultRoles.All.ToList()
            };

            return PartialView("_CreateModal", vm);
        }


        [HttpPost]
        public async Task<IActionResult> CreateModal(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToErrorDictionary());

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = PasswordHasher.Hash(model.Password),
                IsActive = true
            };

            foreach (var role in model.SelectedRoles)
            {
                //user.Roles.Add(new UserRole { Role = role });
            }

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> EditModal(Guid id)
        {
            var user = await _context.Users
                .Include(x => x.Roles)
                .FirstAsync(x => x.Id == id);

            var vm = new UserEditViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                AvailableRoles = DefaultRoles.All.ToList(),
                //SelectedRoles = user.Roles.Select(x => x.Role).ToList()
            };

            return PartialView("_EditModal", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditModal(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToErrorDictionary());

            var user = await _context.Users
                .Include(x => x.Roles)
                .FirstAsync(x => x.Id == model.Id);

            user.Username = model.Username;
            user.Email = model.Email;
            user.IsActive = model.IsActive;

            user.Roles.Clear();

            foreach (var role in model.SelectedRoles)
            {
                //user.Roles.Add(new UserRole { Role = role });
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return NotFound();

            user.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<PartialViewResult> DetailsModal(Guid id)
        {
            var user = await _context.Users
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Id == id);

            return PartialView("_DetailsModal", user);
        }
    }
}
