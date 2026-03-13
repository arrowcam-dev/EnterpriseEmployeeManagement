using EnterpriseEmployeeManagement.Models.Common;

namespace EnterpriseEmployeeManagement.Models
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<UserRole> Users { get; set; } = new List<UserRole>();

    }
}
