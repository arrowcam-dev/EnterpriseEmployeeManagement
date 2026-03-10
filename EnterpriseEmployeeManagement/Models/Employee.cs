using EnterpriseEmployeeManagement.Models.Common;

namespace EnterpriseEmployeeManagement.Models
{
    public class Employee : ITenantEntity, IAuditableEntity, ISoftDelete
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int DepartmentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Position { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }

        public Department Department { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
