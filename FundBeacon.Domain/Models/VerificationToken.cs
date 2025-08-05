using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Domain.Models
{
    public class VerificationToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
