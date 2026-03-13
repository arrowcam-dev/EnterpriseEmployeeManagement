using EnterpriseEmployeeManagement.Models.Common;

namespace EnterpriseEmployeeManagement.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public bool IsActive { get; set; }

        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();

    }
}
