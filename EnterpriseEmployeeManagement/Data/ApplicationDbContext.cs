using EnterpriseEmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EnterpriseEmployeeManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
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
    }
}
