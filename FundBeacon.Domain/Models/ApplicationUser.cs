

using Microsoft.AspNetCore.Identity;

namespace FundBeacon.Domain.Models
{
    public class ApplicationUser:IdentityUser

    {
        public bool IsLive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsDeleted { get; set; } = false;



    }
}
