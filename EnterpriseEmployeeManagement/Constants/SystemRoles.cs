namespace EnterpriseEmployeeManagement.Constants
{
    public static class SystemRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Staff = "Staff";
        public const string Auditor = "Auditor";

        public static readonly string[] All =
        {
            Admin,
            Manager,
            Staff,
            Auditor
        };
    }
}
