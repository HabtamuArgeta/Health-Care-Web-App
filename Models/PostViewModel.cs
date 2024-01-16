namespace HealthCareApp.Models
{
    public class PostViewModel
    {
        public string? Id { get; set; }
        public string DoctorUserName { get; set; }
        public string DatePublished { get; set; } 
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
