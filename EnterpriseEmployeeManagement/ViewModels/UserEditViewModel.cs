using System.ComponentModel.DataAnnotations;

namespace EnterpriseEmployeeManagement.ViewModels
{
    public class UserEditViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public List<string> AvailableRoles { get; set; } = new();

        public List<string> SelectedRoles { get; set; } = new();
    }
}
