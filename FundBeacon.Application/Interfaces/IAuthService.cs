using FundBeacon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(int StatusCode, string Message)> RegisterAsync(RegisterDto model);
        Task<(int StatusCode, string Message)> ConfirmRegistrationAsync(string token);
        Task<(int StatusCode, string Message)> LoginRequestAsync(LoginDto model);
        Task<(int StatusCode, string Message, string? Token, string? RefreshToken)> VerifyOtpAsync(string email, string otp);
    }
}
