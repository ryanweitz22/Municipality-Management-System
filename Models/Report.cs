using System;
using System.ComponentModel.DataAnnotations;

namespace MunicipalityManagementSystem.Models
{
    public class Report
    {
        public int ReportID { get; set; }

        public int CitizenID { get; set; }

        [Required]
        public string ReportType { get; set; }

        [Required]
        public string Details { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Under Review";

        public bool IsFlagged { get; set; } = true; // Flagging Reports until CitizenID is used in ServiceRequest

        public virtual Citizen Citizen { get; set; } // Add a navigation property for the Citizen relationship
    }
}
