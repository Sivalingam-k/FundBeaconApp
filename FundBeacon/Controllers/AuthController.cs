using FundBeacon.Application.Interfaces;
using FundBeacon.Application.Services;
using FundBeacon.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FundBeacon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService auth, ILogger<AuthController> logger)
        {
            _auth = auth;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            _logger.LogInformation("Register attempt for {Email}", model.Email);

            var (code, msg) = await _auth.RegisterAsync(model);

            _logger.LogInformation("Register status {Code}: {Message}", code, msg);
            return StatusCode(code, new { message = msg });
        }

        [HttpGet("confirm-registration")]
        
        public async Task<IActionResult> Confirm([FromQuery] string token)
        {
            _logger.LogInformation("Confirm registration token received");

            var (code, msg) = await _auth.ConfirmRegistrationAsync(token);

            _logger.LogInformation("Email confirmation result: {Message}", msg);
            return StatusCode(code, new { message = msg });
        }

        [HttpPost("login-request")]
        public async Task<IActionResult> LoginRequest([FromBody] LoginDto model)
        {
            _logger.LogInformation("Login request for {Email}", model.Email);

            var (status, message) = await _auth.LoginRequestAsync(model);

            _logger.LogInformation("Login request result: {Status} - {Message}", status, message);

            if (status == 200)
                return Ok(new { message });

            return Unauthorized(new { message });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto model)
        {
            _logger.LogInformation("OTP verification attempt for {Email}", model.Email);

            var (status, message, token,refreshToken) = await _auth.VerifyOtpAsync(model.Email, model.Otp);

            if (status == 200 && token != null)
            {
                _logger.LogInformation("OTP verified successfully for {Email}", model.Email);
                return Ok(new { message, token });
            }

            _logger.LogWarning("OTP verification failed for {Email}: {Message}", model.Email, message);
            return StatusCode(status, new { message });
        }

    }
}
