using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Models
{
    public class AdminViewModel
    {
        
        public string? Id { get; set; }


        public string UserName { get; set; }

        
        public string Address { get; set; }


        public string DateOfBirth { get; set; }

        [EmailAddress]
        public string  Email { get; set; }

        
        public string Gender { get; set; }

        [Phone]
        public string Phone { get; set; }

        public IFormFile Photo { get; set; }    
        public string? PhotoUrl { get; set; }
    }
}
