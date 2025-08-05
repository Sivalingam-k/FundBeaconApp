using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Domain.Models
{
    public class EmailConfirmationPayload
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string? ProviderName { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }

        // Only needed for SUB-PROVIDER
        public int? ParentProviderId { get; set; }
    }
}
