
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FundBeacon.Domain.Models
{
    [Table("OtpVerification")]
    public class OtpVerification
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; } = false;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailSecondary { get; set; }
    }
}
