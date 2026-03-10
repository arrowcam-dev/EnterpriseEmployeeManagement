namespace EnterpriseEmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int DepartmentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Position { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }

        public Department Department { get; set; }
    }
}
