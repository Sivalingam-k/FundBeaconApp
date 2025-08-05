using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Application.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<(bool Success, string Message, string? Token, string? RefreshToken)> HandleGoogleLoginAsync(ExternalLoginInfo info);

    }
}
