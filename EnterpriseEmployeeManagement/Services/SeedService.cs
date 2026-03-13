using EnterpriseEmployeeManagement.Constants;
using EnterpriseEmployeeManagement.Data;
using EnterpriseEmployeeManagement.Helpers;
using EnterpriseEmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

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

            await _context.Database.MigrateAsync();

            await SeedCompanyAsync();
            await SeedAdminUserAsync();

            _tenantProvider.DisableTenantFilter = true;
        }
        private async Task SeedCompanyAsync()
        {
            if (await _context.Companies.IgnoreQueryFilters().AnyAsync())
                return;

            var companyId = Guid.NewGuid();
            var company = new Company
            {
                Id = companyId,
                Name = "Default Company",
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

            var companyId = company.Id;

            var adminRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                CompanyId = companyId,
                Description = "Administrator role with full permissions"
            };

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                Username = "admin",
                Email = "admin@demo.local",
                PasswordHash = PasswordHasher.Hash("Admin@123"),
                IsActive = true
            };

            var userRole = new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            };

            _context.Roles.Add(adminRole);
            _context.Users.Add(adminUser);
            _context.UserRoles.Add(userRole);

            await _context.SaveChangesAsync();
        }
    }
}
