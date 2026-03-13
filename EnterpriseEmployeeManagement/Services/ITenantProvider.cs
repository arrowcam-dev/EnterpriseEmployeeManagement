namespace EnterpriseEmployeeManagement.Services
{
    public interface ITenantProvider
    {
        Guid CompanyId { get; }
        bool DisableTenantFilter { get; set; }
    }
}
