namespace EnterpriseEmployeeManagement.ViewModels
{
    public class EmployeeListViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string DepartmentName { get; set; } = "";

        public string Position { get; set; } = "";

        public bool IsActive { get; set; }
    }
}
