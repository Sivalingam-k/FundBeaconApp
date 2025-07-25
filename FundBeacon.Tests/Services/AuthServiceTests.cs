using FundBeacon.Application.Interfaces;
using FundBeacon.Application.Services;
using FundBeacon.Data;
using FundBeacon.Domain.Models;
using FundBeacon.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FundBeacon.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly FundBeaconDbContext _context;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IConfiguration> _configMock;

        public AuthServiceTests()
        {
            _userManagerMock = MockUserManager<ApplicationUser>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            _emailServiceMock = new Mock<IEmailService>();
            _configMock = new Mock<IConfiguration>();

            var options = new DbContextOptionsBuilder<FundBeaconDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new FundBeaconDbContext(options);

            _configMock.Setup(x => x["App:BaseUrl"]).Returns("http://localhost");
            _configMock.Setup(x => x["Jwt:Key"]).Returns("ThisIsASecretKeyExactly32Chars!!");
            _configMock.Setup(x => x["Jwt:Issuer"]).Returns("testissuer");
            _configMock.Setup(x => x["Jwt:Audience"]).Returns("testaudience");


            _authService = new AuthService(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _context,
                _emailServiceMock.Object,
                _configMock.Object
            );
        }

        // Test: RegisterAsync should send email with token
        [Fact]
        public async Task RegisterAsync_ValidInput_SendsEmail()
        {
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Password = "Test@123",
                Role = "Customer"
            };

            var result = await _authService.RegisterAsync(dto);

            Assert.Equal(200, result.StatusCode);
            Assert.Contains("check your email", result.Message.ToLower());
            _emailServiceMock.Verify(x => x.SendEmailAsync(dto.Email, It.IsAny<string>(), It.Is<string>(body => body.Contains("confirm-registration"))), Times.Once);
        }

        // Test: ConfirmRegistrationAsync should create user and related data
       
        [Fact]
        public async Task ConfirmRegistrationAsync_ValidToken_CreatesUserAndReturnsSuccess()
        {
            var payload = new EmailConfirmationPayload
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                PhoneNumber = "1234567890",
                Password = "Test@123",
                Role = "Customer"
            };

            // Serialize using Newtonsoft.Json to match service code
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            var result = await _authService.ConfirmRegistrationAsync(token);

            Assert.Equal(200, result.StatusCode);
            Assert.Contains("registration complete", result.Message.ToLower());
        }

        [Fact]
        public async Task VerifyOtpAsync_ValidOtp_ReturnsToken()
        {
            var user = new ApplicationUser
            {
                Email = "otp@example.com",
                UserName = "otpuser",
                RefreshToken = null,
                Id = Guid.NewGuid().ToString()
            };

            // Add OTP record to DB
            await _context.OtpVerifications.AddAsync(new OtpVerification
            {
                Email = user.Email,
                Otp = "123456",
                CreatedAt = DateTime.UtcNow,
                IsVerified = false
            });
            await _context.SaveChangesAsync();

            // Setup UserManager mocks
            _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string> { "Customer" });

            var result = await _authService.VerifyOtpAsync(user.Email, "123456");

            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.RefreshToken);
        }


        // Test: LoginRequestAsync with valid credentials
        [Fact]
        public async Task LoginRequestAsync_ValidCredentials_SendsOtp()
        {
            var user = new ApplicationUser
            {
                Email = "login@example.com",
                UserName = "loginuser",
                PhoneNumber = "9876543210"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, "Test@123")).ReturnsAsync(true);

            var result = await _authService.LoginRequestAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Test@123"
            });

            Assert.Equal(200, result.StatusCode);
            Assert.Contains("otp", result.Message.ToLower());
            _emailServiceMock.Verify(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

      

        // Test: VerifyOtpAsync with wrong OTP
        [Fact]
        public async Task VerifyOtpAsync_WrongOtp_ReturnsInvalid()
        {
            var email = "wrongotp@example.com";

            await _context.OtpVerifications.AddAsync(new OtpVerification
            {
                Email = email,
                Otp = "999999",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            var result = await _authService.VerifyOtpAsync(email, "000000");

            Assert.Equal(400, result.StatusCode);
            Assert.Null(result.Token);
        }

        // Test: ConfirmRegistrationAsync with invalid token
        [Fact]
        public async Task ConfirmRegistrationAsync_InvalidToken_ReturnsError()
        {
            var badToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("bad-json"));

            var result = await _authService.ConfirmRegistrationAsync(badToken);

            Assert.Equal(400, result.StatusCode);
        }

        // Utilities
        private static Mock<UserManager<T>> MockUserManager<T>() where T : class
        {
            var store = new Mock<IUserStore<T>>();
            return new Mock<UserManager<T>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
