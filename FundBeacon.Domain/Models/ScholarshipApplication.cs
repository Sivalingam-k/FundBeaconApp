using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Domain.Models
{
    [Table("ScholarshipApplication")]
    public class ScholarshipApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        public int ScholarshipId { get; set; }
        public int CustomerId { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending, Verified, Rejected, Approved

        public string? Notes { get; set; }
        public string? SubmittedDocuments { get; set; } // optional: comma-separated or JSON

        public virtual Scholarship Scholarship { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ScholarshipApplicationVerification? Verification { get; set; }
    }
}
