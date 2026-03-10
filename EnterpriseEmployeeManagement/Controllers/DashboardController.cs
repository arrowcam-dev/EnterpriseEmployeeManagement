using EnterpriseEmployeeManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseEmployeeManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalEmployees = _context.Employees.Count();
            var totalDepartments = _context.Departments.Count();

            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.TotalDepartments = totalDepartments;

            return View();
        }
    }
}
