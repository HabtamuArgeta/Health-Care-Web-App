namespace HealthCareApp.Helper
{
    public class Roles
    {
        public Roles(string role,string name)
        {
            UserRole = role;
            UserName = name;
        }
        public static string UserRole { get; set; } = string.Empty;
        public static string UserName { get; set; } = string.Empty;
    }
}
