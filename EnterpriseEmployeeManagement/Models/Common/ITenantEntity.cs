namespace EnterpriseEmployeeManagement.Models.Common
{
    public interface ITenantEntity
    {
        Guid CompanyId { get; set; }
    }
}
