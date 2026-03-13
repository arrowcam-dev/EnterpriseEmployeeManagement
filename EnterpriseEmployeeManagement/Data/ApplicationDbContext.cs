using EnterpriseEmployeeManagement.Models;
using EnterpriseEmployeeManagement.Models.Common;
using EnterpriseEmployeeManagement.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        
        private void SetTenantFilter<TEntity>(ModelBuilder builder)
            where TEntity : class, ITenantEntity, ISoftDelete
        {
            builder.Entity<TEntity>()
                .HasQueryFilter(x =>
                !_tenantProvider.DisableTenantFilter ? (x.CompanyId == _tenantProvider.CompanyId && !x.IsDeleted) : !x.IsDeleted);
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(r => r.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(x => x.RoleId);

            //modelBuilder.Entity<Employee>()
            //    .HasQueryFilter(x =>
            //        !_tenantProvider.DisableTenantFilter ? (x.CompanyId == _tenantProvider.CompanyId && !x.IsDeleted) : !x.IsDeleted);

            //modelBuilder.Entity<Department>()
            //    .HasQueryFilter(x =>
            //        !_tenantProvider.DisableTenantFilter ? (x.CompanyId == _tenantProvider.CompanyId && !x.IsDeleted) : !x.IsDeleted);

            //modelBuilder.Entity<User>()
            //    .HasQueryFilter(x =>
            //        !_tenantProvider.DisableTenantFilter ? (x.CompanyId == _tenantProvider.CompanyId && !x.IsDeleted) : !x.IsDeleted);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext)
                        .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var companyId = _tenantProvider.CompanyId;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ITenantEntity tenantEntity &&
                    entry.State == EntityState.Added)
                {
                    if (!_tenantProvider.DisableTenantFilter)
                    {
                        tenantEntity.CompanyId = companyId;
                    }
                }

                if (entry.Entity is IAuditableEntity auditEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditEntity.CreatedDate = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        auditEntity.UpdatedDate = DateTime.UtcNow;
                    }
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
