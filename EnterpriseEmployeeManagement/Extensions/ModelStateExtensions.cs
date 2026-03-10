using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnterpriseEmployeeManagement.Extensions
{
    public static class ModelStateExtensions
    {
        public static Dictionary<string, string[]> ToErrorDictionary(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray()
                );
        }
    }
}
