namespace EnterpriseEmployeeManagement.ViewModels
{
    public class UserListViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<UserRoleViewModel> Roles { get; set; } = new List<UserRoleViewModel>();
    }
}
