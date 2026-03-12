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
        private readonly ITenantProvider _tenantProvider;
        public SeedService(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task SeedAsync()
        {
            _tenantProvider.DisableTenantFilter = true;

            await SeedCompanyAsync();
            await SeedAdminUserAsync();

            _tenantProvider.DisableTenantFilter = true;
        }
        private async Task SeedCompanyAsync()
        {
            if (await _context.Companies.IgnoreQueryFilters().AnyAsync())
                return;

            var company = new Company
            {
                Name = "Demo Company",
                CreatedDate = DateTime.UtcNow
            };

            _context.Companies.Add(company);

            await _context.SaveChangesAsync();
        }

        private async Task SeedAdminUserAsync()
        {
            if (await _context.Users.IgnoreQueryFilters().AnyAsync())
                return;

            var company = await _context.Companies.IgnoreQueryFilters().FirstAsync();

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
