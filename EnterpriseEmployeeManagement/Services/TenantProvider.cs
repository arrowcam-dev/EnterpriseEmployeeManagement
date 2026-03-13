namespace EnterpriseEmployeeManagement.Services
{
    public class TenantProvider : ITenantProvider
    {
        //public int GetCompanyId()
        //{
        //    // Temporary demo tenant
        //    return 1;
        //}
        public int CompanyId { get; set; }
        public bool DisableTenantFilter { get; set; }
    }
}
