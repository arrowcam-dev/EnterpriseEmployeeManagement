namespace EnterpriseEmployeeManagement.Services
{
    public interface ITenantProvider
    {
        //int GetCompanyId();
        int CompanyId { get; set; }
        bool DisableTenantFilter { get; set; }
    }
}
