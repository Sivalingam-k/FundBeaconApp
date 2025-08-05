using FundBeacon.Application.Interfaces;
using FundBeacon.Data;
using FundBeacon.Domain.Models;
using FundBeacon.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FundBeacon.Application.Services
{
    public class AuthService : IAuthService
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;   // Add this
        //private readonly FundBeaconDbContext _context;
        //private readonly IEmailService _emailService;
        //private readonly IConfiguration _configuration;

        //public AuthService(
        //    UserManager<ApplicationUser> userManager,
        //    RoleManager<IdentityRole> roleManager,          // Inject here
        //    FundBeaconDbContext context,
        //    IEmailService emailService,
        //    IConfiguration configuration)
        //{
        //    _userManager = userManager;
        //    _roleManager = roleManager;                      // Assign here
        //    _context = context;
        //    _emailService = emailService;
        //    _configuration = configuration;
        //}

        //public async Task<(int StatusCode, string Message)> RegisterAsync(RegisterDto model)
        //{
        //    var payload = new EmailConfirmationPayload
        //    {
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        Email = model.Email,
        //        PhoneNumber = model.PhoneNumber,
        //        Password = model.Password,
        //        Role = model.Role
        //    };

        //    var json = JsonConvert.SerializeObject(payload);
        //    var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        //    var baseUrl = _configuration["App:BaseUrl"];
        //    var confirmUrl = $"{baseUrl}/api/auth/confirm-registration?token={Uri.EscapeDataString(token)}";

        //    await _emailService.SendEmailAsync(
        //        model.Email,
        //        "Confirm your email",
        //        $"Click <a href='{confirmUrl}'>here</a> to complete registration."
        //    );

        //    return (200, "Please check your email to confirm your registration.");
        //}



        //public async Task<(int StatusCode, string Message)> ConfirmRegistrationAsync(string token)
        //{
        //    EmailConfirmationPayload payload;
        //    try
        //    {
        //        var json = Encoding.UTF8.GetString(Convert.FromBase64String(Uri.UnescapeDataString(token)));
        //        payload = JsonConvert.DeserializeObject<EmailConfirmationPayload>(json);
        //        if (payload == null) throw new Exception("Invalid data");
        //    }
        //    catch
        //    {
        //        return (400, "Invalid or expired token.");
        //    }

        //    var tx = _context.Database.IsRelational() ? await _context.Database.BeginTransactionAsync() : null;

        //    try
        //    {
        //        // ✅ Validate role
        //        var allowedRoles = new[] { "STUDENT", "SCHOLORSHIP-PROVIDER", "SUB-SCHOLORSHIP-PROVIDER", "ADMIN" };
        //        if (!allowedRoles.Contains(payload.Role?.ToUpper()))
        //        {
        //            if (tx != null) await tx.RollbackAsync();
        //            return (400, $"Invalid role: {payload.Role}");
        //        }

        //        var baseUserName = $"{payload.FirstName}{payload.LastName}".ToLower();
        //        var userName = baseUserName;
        //        var suffix = 1;
        //        while (await _userManager.FindByNameAsync(userName) != null)
        //            userName = $"{baseUserName}{suffix++}";

        //        var user = new ApplicationUser
        //        {
        //            UserName = userName,
        //            Email = payload.Email,
        //            PhoneNumber = payload.PhoneNumber,
        //            EmailConfirmed = true
        //        };

        //        var result = await _userManager.CreateAsync(user, payload.Password);
        //        if (!result.Succeeded)
        //        {
        //            if (tx != null) await tx.RollbackAsync();
        //            return (400, string.Join("; ", result.Errors.Select(e => e.Description)));
        //        }

        //        // ✅ STUDENT Role
        //        if (payload.Role.Equals("STUDENT", StringComparison.OrdinalIgnoreCase))
        //        {
        //            var customer = new Customer
        //            {
        //                User = user,
        //                CustomerCode = $"CUST-{Guid.NewGuid():N}".Substring(0, 10).ToUpper(),
        //                UniqueId = new Random().Next(100_000_000, 999_999_999).ToString(),
        //                Associations = new[]
        //                {
        //            new CustomerAssociation
        //            {
        //                AssociatedCode = "ASSOC-CODE",
        //                AssociationStartDate = DateTime.UtcNow
        //            }
        //        }
        //            };

        //            var contact = new Contact
        //            {
        //                FirstName = payload.FirstName,
        //                LastName = payload.LastName,
        //                EmailSecondary = payload.Email,
        //                PhoneMobile = payload.PhoneNumber,
        //                UserName = user.UserName,
        //                Customer = customer,
        //                CreatedBy = "System",
        //                CreatedOn = DateTime.UtcNow,
        //                AssociatedCode = customer.CustomerCode,
        //                Gender = "Unspecified",
        //                PhoneHome = string.Empty,
        //                PhoneWork = string.Empty,
        //                PhoneFax = string.Empty,
        //                PreferredContactMethod = "Email",
        //                StatusChangeDate = DateTime.UtcNow,
        //                StatusChangeReason = "New Registration",
        //                Group = "Default",
        //                Title = ""
        //            };

        //            _context.Customers.Add(customer);
        //            _context.Contacts.Add(contact);
        //        }

        //        // ✅ SCHOLARSHIP-PROVIDER Role
        //        else if (payload.Role.Equals("SCHOLORSHIP-PROVIDER", StringComparison.OrdinalIgnoreCase))
        //        {
        //            var provider = new ScholarshipProvider
        //            {
        //                Name = payload.ProviderName ?? $"{payload.FirstName} {payload.LastName}",
        //                ContactEmail = payload.Email,
        //                Phone = payload.PhoneNumber,
        //                Description = "Registered via portal",
        //                Website = payload.Website,
        //                Address = payload.Address
        //            };

        //            _context.ScholarshipProviders.Add(provider);
        //        }

        //        // ✅ SUB-PROVIDER Role
        //        else if (payload.Role.Equals("SUB-SCHOLORSHIP-PROVIDER", StringComparison.OrdinalIgnoreCase))
        //        {
        //            if (payload.ParentProviderId == null)
        //            {
        //                if (tx != null) await tx.RollbackAsync();
        //                return (400, "Parent provider ID is required for sub-provider.");
        //            }

        //            var subProvider = new ScholarshipProvider
        //            {
        //                Name = payload.ProviderName ?? $"{payload.FirstName} {payload.LastName}",
        //                ContactEmail = payload.Email,
        //                Phone = payload.PhoneNumber,
        //                Description = "Registered as sub-provider",
        //                Website = payload.Website,
        //                Address = payload.Address,
        //                ParentProviderId = payload.ParentProviderId
        //            };

        //            _context.ScholarshipProviders.Add(subProvider);
        //        }

        //        // ✅ Optional ADMIN logic (currently just allow user creation and role assignment)

        //        await _context.SaveChangesAsync();

        //        if (tx != null)
        //            await tx.CommitAsync();

        //        await _userManager.AddToRoleAsync(user, payload.Role);

        //        return (200, "Email confirmed and registration complete!");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (tx != null)
        //            await tx.RollbackAsync();

        //        return (500, $"Registration failed: {ex.Message}");
        //    }
        //}


        //public async Task<(int StatusCode, string Message)> LoginRequestAsync(LoginDto model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        //        return (401, "Invalid email or password.");

        //    var otp = new Random().Next(100000, 999999).ToString();

        //    var otpEntry = new OtpVerification
        //    {
        //        Email = user.Email,
        //        Otp = otp,
        //        CreatedAt = DateTime.UtcNow,
        //        FirstName = user.UserName,
        //        PhoneNumber = user.PhoneNumber, 
        //        EmailSecondary = user.Email
        //    };

        //    _context.OtpVerifications.Add(otpEntry);
        //    await _context.SaveChangesAsync();

        //    await _emailService.SendEmailAsync(user.Email, "Your OTP Code", $"Your OTP is <b>{otp}</b>. It expires in 5 minutes.");

        //    return (200, "OTP sent to your email.");
        //}

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly FundBeaconDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            FundBeaconDbContext context,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

 

        public async Task<(int StatusCode, string Message)> RegisterAsync(RegisterDto model)
        {
            using var tx = _context.Database.IsRelational() ? await _context.Database.BeginTransactionAsync() : null;

            try
            {
                var baseUserName = $"{model.FirstName}{model.LastName}".ToLower();
                var userName = baseUserName;
                var suffix = 1;
                while (await _userManager.FindByNameAsync(userName) != null)
                    userName = $"{baseUserName}{suffix++}";

                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = false,
                    IsVerified = false,
                    IsLive = true,
                    IsDeleted = false
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return (400, string.Join("; ", result.Errors.Select(e => e.Description)));

                await _userManager.AddToRoleAsync(user, model.Role);

                if (model.Role == "SCHOLORSHIP-PROVIDER")
                {
                    var provider = new ScholarshipProvider
                    {
                        Name = $"{model.FirstName} {model.LastName}",
                        ContactEmail = model.Email,
                        Phone = model.PhoneNumber
                    };

                    _context.ScholarshipProviders.Add(provider);
                }
                else if (model.Role == "SUB-SCHOLORSHIP-PROVIDER")
                {
                    if (model.ParentProviderId == null)
                        return (400, "Parent provider ID is required for sub-provider.");

                    var provider = new ScholarshipProvider
                    {
                        Name = $"{model.FirstName} {model.LastName}",
                        ContactEmail = model.Email,
                        Phone = model.PhoneNumber,
                        ParentProviderId = model.ParentProviderId.Value
                    };

                    _context.ScholarshipProviders.Add(provider);
                }
                else
                {
                    var customer = new Customer
                    {
                        User = user,
                        CustomerCode = $"CUST-{Guid.NewGuid():N}".Substring(0, 10).ToUpper(),
                        UniqueId = new Random().Next(100_000_000, 999_999_999).ToString(),
                        Associations = new[]
                        {
                    new CustomerAssociation
                    {
                        AssociatedCode = "ASSOC-CODE",
                        AssociationStartDate = DateTime.UtcNow
                    }
                }
                    };

                    var contact = new Contact
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        EmailSecondary = model.Email,
                        PhoneMobile = model.PhoneNumber,
                        UserName = user.UserName,
                        Customer = customer,
                        CreatedBy = "System",
                        CreatedOn = DateTime.UtcNow,
                        AssociatedCode = customer.CustomerCode,
                        Gender = "Unspecified",
                        PhoneHome = string.Empty,
                        PhoneWork = string.Empty,
                        PhoneFax = string.Empty,
                        PreferredContactMethod = "Email",
                        StatusChangeDate = DateTime.UtcNow,
                        StatusChangeReason = "New Registration",
                        Group = "Default",
                        Title = ""
                    };

                    _context.Customers.Add(customer);
                    _context.Contacts.Add(contact);
                }

                await _context.SaveChangesAsync();

                var tokenValue = Guid.NewGuid().ToString("N");
                var expiry = DateTime.UtcNow.AddHours(24);

                var verificationToken = new VerificationToken
                {
                    UserId = user.Id,
                    Token = tokenValue,
                    ExpiryTime = expiry
                };

                _context.VerificationTokens.Add(verificationToken);
                await _context.SaveChangesAsync();

                if (tx != null)
                    await tx.CommitAsync();

                var baseUrl = _configuration["App:BaseUrl"];
                var confirmUrl = $"{baseUrl}/api/auth/confirm-registration?token={Uri.EscapeDataString(tokenValue)}";

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Activate Your Fund-Beacon Account",
                    $"Click <a href='{confirmUrl}'>here</a> to activate your account. This link expires in 24 hours.");

                return (200, "Registration successful. Please check your email to activate your account.");
            }
            catch (Exception ex)
            {
                if (tx != null)
                    await tx.RollbackAsync();
                return (500, $"Registration failed: {ex.Message}");
            }
        }


        public async Task<(int StatusCode, string Message)> ConfirmRegistrationAsync(string token)
        {
            var tokenEntity = await _context.VerificationTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsUsed);

            if (tokenEntity == null)
                return (400, "Invalid or already used token.");

            if (tokenEntity.ExpiryTime < DateTime.UtcNow)
            {
                // Expired → generate and resend new token
                var newToken = Guid.NewGuid().ToString("N");
                var expiry = DateTime.UtcNow.AddHours(24);

                var newVerificationToken = new VerificationToken
                {
                    UserId = tokenEntity.UserId,
                    Token = newToken,
                    ExpiryTime = expiry
                };

                _context.VerificationTokens.Add(newVerificationToken);
                await _context.SaveChangesAsync();

                var baseUrl = _configuration["App:BaseUrl"];
                var confirmUrl = $"{baseUrl}/api/auth/confirm-registration?token={Uri.EscapeDataString(newToken)}";

                await _emailService.SendEmailAsync(
                    tokenEntity.User.Email,
                    "Your Activation Link Expired",
                    $"Your previous link expired. Click <a href='{confirmUrl}'>here</a> to activate your account. This new link expires in 24 hours.");

                return (400, "The link has expired. A new activation link has been sent.");
            }

            // ✅ Activate the user
            tokenEntity.IsUsed = true;
            tokenEntity.User.EmailConfirmed = true;
            tokenEntity.User.IsVerified = true;

            await _context.SaveChangesAsync();

            return (200, "Your account has been activated successfully!");
        }



        public async Task<(int StatusCode, string Message)> LoginRequestAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return (401, "Invalid email or password.");

            // Check email confirmed & manually verified
            if (!user.EmailConfirmed || !user.IsVerified)
            {
                var existing = await _context.VerificationTokens
                    .Where(v => v.UserId == user.Id && !v.IsUsed)
                    .OrderByDescending(v => v.ExpiryTime)
                    .FirstOrDefaultAsync();

                bool needResend = existing == null || existing.ExpiryTime < DateTime.UtcNow;

                if (needResend)
                {
                    var token = Guid.NewGuid().ToString();
                    var vt = new VerificationToken
                    {
                        UserId = user.Id,
                        Token = token,
                        ExpiryTime = DateTime.UtcNow.AddHours(24)
                    };
                    _context.VerificationTokens.Add(vt);
                    await _context.SaveChangesAsync();

                    var confirmUrl = $"{_configuration["App:BaseUrl"]}/api/auth/confirm-registration?token={Uri.EscapeDataString(token)}";
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Activate your account",
                        $"Click <a href='{confirmUrl}'>here</a> to activate your account."
                    );
                }

                return (403, "Account not activated. Check your email to activate.");
            }

            // ✅ Generate 6-digit OTP
            var otp = new Random().Next(100000, 999999).ToString();
            var otpRecord = new OtpVerification
            {
                Email = user.Email,
                Otp = otp,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.OtpVerifications.Add(otpRecord);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                user.Email,
                "Your OTP for Fund-Beacon Login",
                $"Your OTP is: <strong>{otp}</strong>. It is valid for 5 minutes."
            );

            return (200, "OTP sent to your email. Please verify to complete login.");
        }


        public async Task<(int StatusCode, string Message, string? Token, string? RefreshToken)> VerifyOtpAsync(string email, string otp)
        {
            var otpRecord = await _context.OtpVerifications
                .Where(o => o.Email == email && !o.IsVerified)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otpRecord == null || otpRecord.Otp != otp || (DateTime.UtcNow - otpRecord.CreatedAt).TotalMinutes > 5)
                return (400, "OTP is invalid or expired.", null, null);

            otpRecord.IsVerified = true;
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (404, "User not found.", null, null);

            user.LastLogin = DateTime.UtcNow;

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = GenerateJwtToken(user, roles);

            return (200, "OTP verified. Login successful.", jwtToken, refreshToken);
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        public string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

      
    }
}
