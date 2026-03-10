using EnterpriseEmployeeManagement.Models;
using EnterpriseEmployeeManagement.Models.Common;
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
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var companyId = _tenantProvider.GetCompanyId();

            foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CompanyId = companyId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
