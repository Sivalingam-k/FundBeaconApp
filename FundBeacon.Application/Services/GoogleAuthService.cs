using FundBeacon.Application.Interfaces;
using FundBeacon.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Application.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _tokenService;

        public GoogleAuthService(UserManager<ApplicationUser> userManager, IAuthService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<(bool Success, string Message, string? Token, string? RefreshToken)> HandleGoogleLoginAsync(ExternalLoginInfo info)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return (false, "Email not found in Google login", null, null);

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    IsVerified = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return (false, "Failed to create user", null, null);

                await _userManager.AddToRoleAsync(user, "customer");
            }

            // Link login if not already linked
            var login = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (login == null)
                await _userManager.AddLoginAsync(user, info);

            // Generate tokens
            var roles = await _userManager.GetRolesAsync(user);
            var jwt = _tokenService.GenerateJwtToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (true, "Login via Google successful", jwt, refreshToken);
        }
    }
}
