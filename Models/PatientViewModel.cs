using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Models
{
    public class PatientViewModel
    {
        public string? Id { get; set; }

       
        public string UserName { get; set; }
        
        public string Password { get; set; }

        
        [EmailAddress]
        public string Email { get; set; }

        
        [Phone]
        public string Phone { get; set; }

        public IFormFile Photo { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
