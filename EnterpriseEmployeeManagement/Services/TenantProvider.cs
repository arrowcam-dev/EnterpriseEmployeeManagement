namespace EnterpriseEmployeeManagement.Services
{
    public class TenantProvider : ITenantProvider
    {
        public int GetCompanyId()
        {
            // Temporary demo tenant
            return 1;
        }
    }
}
