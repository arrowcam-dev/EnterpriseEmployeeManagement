using EnterpriseEmployeeManagement.Constants;
using EnterpriseEmployeeManagement.Data;
using EnterpriseEmployeeManagement.Helpers;
using EnterpriseEmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeManagement.Services
{
    public class SeedService: ISeedService
    {
        private readonly ApplicationDbContext _context;

        public SeedService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedCompanyAsync();
            await SeedAdminUserAsync();
        }
        private async Task SeedCompanyAsync()
        {
            if (await _context.Companies.AnyAsync())
                return;

            var company = new Company
            {
                Name = "Demo Company"
            };

            _context.Companies.Add(company);

            await _context.SaveChangesAsync();
        }

        private async Task SeedAdminUserAsync()
        {
            if (await _context.Users.AnyAsync())
                return;

            var company = await _context.Companies.FirstAsync();

            var user = new User
            {
                CompanyId = company.Id,
                Username = "admin",
                Email = "admin@demo.local",
                PasswordHash = PasswordHasher.Hash("Admin@123"),
                IsActive = true
            };

            user.Roles.Add(new UserRole
            {
                Role = SystemRoles.Admin
            });

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }
    }
}
