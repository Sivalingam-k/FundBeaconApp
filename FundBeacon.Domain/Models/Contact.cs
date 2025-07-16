
using System.ComponentModel.DataAnnotations;


namespace FundBeacon.Domain.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        public string? Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime? Dob { get; set; }

        [Required]
        public string Gender { get; set; }

        public string? PhoneMobile { get; set; }
        public string? PhoneHome { get; set; }
        public string? PhoneWork { get; set; }
        public string? PhoneFax { get; set; }

        public string? EmailSecondary { get; set; }

        public string? PreferredContactMethod { get; set; }
        public string? UserName { get; set; }

        public DateTime? StatusChangeDate { get; set; }
        public string? StatusChangeReason { get; set; }

        public string AssociatedCode { get; set; }

        public int CustomerId { get; set; }

        public int Age { get; set; }

        public string? Group { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
