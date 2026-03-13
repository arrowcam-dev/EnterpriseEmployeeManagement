using System.Security.Claims;

namespace EnterpriseEmployeeManagement.Helpers
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userIdClaim == null) return 0; // or throw an exception if you prefer

            return int.Parse(userIdClaim);
        }

        public static int GetCompanyId(this ClaimsPrincipal user)
        {
            var companyIdClaim = user.FindFirstValue("CompanyId");
            if(companyIdClaim == null) return 0; // or throw an exception if you prefer
            return int.Parse(companyIdClaim);
        }
    }
}
