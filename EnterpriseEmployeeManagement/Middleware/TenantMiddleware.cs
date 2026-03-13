using EnterpriseEmployeeManagement.Services;

namespace EnterpriseEmployeeManagement.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var companyIdClaim = context.User.FindFirst("CompanyId");

                if (companyIdClaim != null &&
                    Guid.TryParse(companyIdClaim.Value, out Guid companyId))
                {
                    //tenantProvider.CompanyId = companyId;
                }
            }

            await _next(context);
        }
    }
}
