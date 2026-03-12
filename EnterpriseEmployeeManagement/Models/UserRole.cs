namespace EnterpriseEmployeeManagement.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Role { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
