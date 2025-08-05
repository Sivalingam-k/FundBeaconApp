using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FundBeacon.Domain.Models

{
    [Table("Scholarship")]
    public class Scholarship
    {
        [Key]
        public int ScholarshipId { get; set; }

        [Required]
        public string Title { get; set; }

        public string About { get; set; }
        public string Eligibility { get; set; }
        public DateTime Deadline { get; set; }
        public string Benefits { get; set; }
        public string DocumentsRequired { get; set; }
        public string HowCanYouApply { get; set; }
        public string ContactUs { get; set; }
        public string Disclaimer { get; set; }

        public int ProviderId { get; set; }

        [JsonIgnore]
        public virtual ScholarshipProvider Provider { get; set; }

        [JsonIgnore]
        public virtual ICollection<ScholarshipApplication> Applications { get; set; }

        public bool IsDeleted { get; set; } = false;
    }

    
}
