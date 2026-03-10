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

            modelBuilder.Entity<Employee>()
                .HasQueryFilter(e =>
                    !e.IsDeleted &&
                    e.CompanyId == _tenantProvider.GetCompanyId());

            modelBuilder.Entity<Department>()
                .HasQueryFilter(d =>
                    !d.IsDeleted &&
                    d.CompanyId == _tenantProvider.GetCompanyId());

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var companyId = _tenantProvider.GetCompanyId();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ITenantEntity tenantEntity &&
                    entry.State == EntityState.Added)
                {
                    tenantEntity.CompanyId = companyId;
                }

                if (entry.Entity is IAuditableEntity auditEntity)
                {
                    if (entry.State == EntityState.Added)
                        auditEntity.CreatedDate = DateTime.UtcNow;

                    if (entry.State == EntityState.Modified)
                        auditEntity.UpdatedDate = DateTime.UtcNow;
                }

                if (entry.Entity is ISoftDelete softDelete)
                {
                    if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        softDelete.IsDeleted = true;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
