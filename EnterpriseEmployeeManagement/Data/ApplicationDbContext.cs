using EnterpriseEmployeeManagement.Models;
using EnterpriseEmployeeManagement.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EnterpriseEmployeeManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasData(
                 new Company
                 {
                     Id = 1,
                     Name = "Demo Company",
                     CreatedDate = new DateTime(2025, 1, 1)
                 }
             );

            modelBuilder.Entity<Employee>()
                .HasQueryFilter(e => e.CompanyId == _tenantProvider.GetCompanyId());

            modelBuilder.Entity<Department>()
                .HasQueryFilter(d => d.CompanyId == _tenantProvider.GetCompanyId());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var companyId = _tenantProvider.GetCompanyId();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is Department dept)
                        dept.CompanyId = companyId;

                    if (entry.Entity is Employee emp)
                        emp.CompanyId = companyId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
