using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseEmployeeManagement.ViewModels
{
    public class EmployeeFormViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public Guid DepartmentId { get; set; }

        public string Position { get; set; } = "";

        [Required]
        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }

        public List<SelectListItem> Departments { get; set; } = new();
    }
}
