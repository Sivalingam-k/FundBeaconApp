using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Domain.Models
{
    public class ScholarshipApplicationVerification
    {
        [Key]
        public int VerificationId { get; set; }

        public int ApplicationId { get; set; }
        public int SubProviderId { get; set; } // ScholarshipProvider (who is a sub-provider)

        public DateTime VerifiedOn { get; set; } = DateTime.UtcNow;
        public string VerificationStatus { get; set; } // Verified, Rejected, Pending
        public string? Remarks { get; set; }

        public virtual ScholarshipApplication Application { get; set; }
        public virtual ScholarshipProvider SubProvider { get; set; }
    }
}
