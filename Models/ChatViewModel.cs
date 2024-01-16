namespace HealthCareApp.Models
{
    public class ChatViewModel
    {
        public string? Id { get; set; }
        
        public string DoctorUserName { get; set; }
       
        public string PatientUserName { get; set; }
    
        public string CreatedAt { get; set; }

        public string Message { get; set; }
    }
}
