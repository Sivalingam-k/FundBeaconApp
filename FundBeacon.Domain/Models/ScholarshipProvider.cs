using System.ComponentModel.DataAnnotations;

namespace FundBeacon.Domain.Models
{
    public class ScholarshipProvider
    {
        [Key]
        public int ProviderId { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? ContactEmail { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }

        public int? ParentProviderId { get; set; }
        public virtual ScholarshipProvider? ParentProvider { get; set; }

        public virtual ICollection<ScholarshipProvider> SubProviders { get; set; } = new List<ScholarshipProvider>();
        public virtual ICollection<Scholarship> Scholarships { get; set; } = new List<Scholarship>();
    }
}
