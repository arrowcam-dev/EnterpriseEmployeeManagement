namespace EnterpriseEmployeeManagement.Services
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CompanyId
        {
            get
            {
                if(_httpContextAccessor.HttpContext == null || !_httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    return Guid.Empty;
                }

                var claim = _httpContextAccessor.HttpContext.User.FindFirst("CompanyId");

                return claim == null ? Guid.Empty : Guid.Parse(claim.Value);
            }
        }

        public bool DisableTenantFilter {get; set; } =false;
    }
}
