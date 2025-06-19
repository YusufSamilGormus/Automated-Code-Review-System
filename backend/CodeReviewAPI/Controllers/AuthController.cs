using CodeReviewAPI.DTOs;
using CodeReviewAPI.Data;
using CodeReviewAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.Login(loginDto);
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] RegisterDto registerDto)
        {
            var token = await _authService.Register(registerDto);
            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshToken([FromBody] string refreshToken)
        {
            var token = await _authService.RefreshToken(refreshToken);
            return Ok(token);
        }

        [HttpPost("revoke-token")]
        public async Task<ActionResult> RevokeToken([FromBody] string token)
        {
            await _authService.RevokeToken(token);
            return Ok();
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized();

            user.RefreshToken = null;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Logged out successfully." });
        }

    }
}
