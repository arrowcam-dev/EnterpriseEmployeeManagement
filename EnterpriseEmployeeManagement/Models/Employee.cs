using EnterpriseEmployeeManagement.Models.Common;

namespace EnterpriseEmployeeManagement.Models
{
    public class Employee : BaseEntity
    {

        public Guid DepartmentId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }

        public Department Department { get; set; } = null!;

    }
}
