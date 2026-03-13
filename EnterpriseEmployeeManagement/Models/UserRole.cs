using EnterpriseEmployeeManagement.Models.Common;

namespace EnterpriseEmployeeManagement.Models
{
    public class UserRole: ITenantEntity, ISoftDelete
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public User User { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public Guid CompanyId { get; set; }
    }
}
