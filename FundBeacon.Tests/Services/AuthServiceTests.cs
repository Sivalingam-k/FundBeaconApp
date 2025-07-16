using FundBeacon.Application.Interfaces;
using FundBeacon.Application.Services;
using FundBeacon.Data;
using FundBeacon.Domain.Models;
using FundBeacon.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
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
            // Setup mocks
            _userManagerMock = MockUserManager<ApplicationUser>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            _emailServiceMock = new Mock<IEmailService>();
            _configMock = new Mock<IConfiguration>();

            // In-memory EF DB
            var options = new DbContextOptionsBuilder<FundBeaconDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new FundBeaconDbContext(options);

            // Seed a user
            var user = new ApplicationUser
            {
                Email = "test@example.com",
                UserName = "testuser",
                PhoneNumber = "1234567890"
            };
            _userManagerMock.Setup(um => um.FindByEmailAsync("test@example.com"))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, "Test@123"))
                            .ReturnsAsync(true);

            _authService = new AuthService(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _context,
                _emailServiceMock.Object,
                _configMock.Object
            );
        }

        [Fact]  
        public async Task LoginRequestAsync_WithValidCredentials_SendsOtp()
        {
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "Test@123"
            };

            var result = await _authService.LoginRequestAsync(loginDto);

            Assert.Equal(200, result.StatusCode);
            Assert.Equal("OTP sent to your email.", result.Message);
        }

        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}

