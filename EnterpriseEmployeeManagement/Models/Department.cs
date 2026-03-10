namespace EnterpriseEmployeeManagement.Models
{
    public class Department
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
