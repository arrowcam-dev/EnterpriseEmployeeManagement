using EnterpriseEmployeeManagement.Models.Common;

namespace EnterpriseEmployeeManagement.Models
{
    public class User : ITenantEntity, IAuditableEntity, ISoftDelete
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public bool IsActive { get; set; }

        public List<UserRole> Roles { get; set; } = new();

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
