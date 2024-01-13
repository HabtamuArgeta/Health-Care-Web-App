using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Models
{
    public class DocterViewModel
    {

        
        public string? Id { get; set; }

       
        public string UserName { get; set; }
       
        public string Password { get; set; }


        public string Speciality { get; set; }

        [EmailAddress]
        public string Email { get; set; }

       
        public string Gender { get; set; }

       
        [Phone]
        public string Phone { get; set; }

        
        public string? PhotoUrl { get; set; }
        IFormFile Photo { get; set; }
        
   
    }
}
