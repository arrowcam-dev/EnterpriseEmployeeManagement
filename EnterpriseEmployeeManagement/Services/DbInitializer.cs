using EnterpriseEmployeeManagement.Constants;
using EnterpriseEmployeeManagement.Data;
using EnterpriseEmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeManagement.Services
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public DbInitializer(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task InitializeAsync()
        {
            _tenantProvider.DisableTenantFilter = true;
            await _context.Database.MigrateAsync();

            if (await _context.Companies.AnyAsync())
                return;

            var companyId = Guid.NewGuid();

            var company = new Company
            {
                Id = companyId,
                Name = "Default Company",
                Domain = "defaultcompany.com",
                IsActive = true,
            };

            var roles = new List<Role>
        {
            new Role { Id = Guid.NewGuid(), Name = DefaultRoles.Admin, CompanyId = companyId },
            new Role { Id = Guid.NewGuid(), Name = DefaultRoles.HR, CompanyId = companyId },
            new Role { Id = Guid.NewGuid(), Name = DefaultRoles.Manager, CompanyId = companyId },
            new Role { Id = Guid.NewGuid(), Name = DefaultRoles.Employee, CompanyId = companyId }
        };

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@company.com",
                FullName = "System Admin",
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                CompanyId = companyId
            };

            var adminRole = roles.First(r => r.Name == "Admin");

            var userRole = new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
                CompanyId = companyId
            };

            await _context.Companies.AddAsync(company);
            await _context.Roles.AddRangeAsync(roles);
            await _context.Users.AddAsync(adminUser);
            await _context.UserRoles.AddAsync(userRole);

            await _context.SaveChangesAsync();

            _tenantProvider.DisableTenantFilter = false;
        }
    }
}
