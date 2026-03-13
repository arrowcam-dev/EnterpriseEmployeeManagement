namespace EnterpriseEmployeeManagement.Models.Common
{
    public abstract class BaseEntity :
        ITenantEntity,
        IAuditableEntity,
        ISoftDelete
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
