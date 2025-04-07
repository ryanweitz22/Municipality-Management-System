namespace MunicipalityManagementSystem.Models
{
    public class ServiceRequest
    {
        public int RequestID { get; set; }
        public int CitizenID { get; set; }
        public Citizen Citizen { get; set; } // Correct navigation property
        public string ServiceType { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
    }
}
