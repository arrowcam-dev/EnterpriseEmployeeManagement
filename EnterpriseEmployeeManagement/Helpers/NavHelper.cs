using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnterpriseEmployeeManagement.Helpers
{
    public static class NavHelper
    {
        public static string IsActive(this ViewContext viewContext, string controller)
        {
            var routeData = viewContext.RouteData.Values["controller"]?.ToString();

            if (routeData == null || controller == null) {
                return "";
            }

            return routeData.Equals(controller, StringComparison.InvariantCultureIgnoreCase) ? "active" : "";
        }
    }
}
