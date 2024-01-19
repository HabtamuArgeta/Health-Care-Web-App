namespace HealthCareApp.Helper
{
    public class Roles
    {
        public Roles(string role)
        {
            UserRole = role;
        }
        public static string UserRole { get; set; } = string.Empty;
    }
}
