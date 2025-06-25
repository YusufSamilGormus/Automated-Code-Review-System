using CodeReviewAPI.DTOs;
using CodeReviewAPI.Models;
using CodeReviewAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _jwtSecret;
        private readonly string _refreshTokenSecret;
        private readonly IEmailService _emailService;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _jwtSecret = _configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret not configured");
            _refreshTokenSecret = _configuration["Jwt:RefreshTokenSecret"] ?? throw new ArgumentNullException("Jwt:RefreshTokenSecret not configured");
        }

        private string GenerateEmailVerificationToken()
        {
            return Guid.NewGuid().ToString();
        }


        public async Task<TokenResponse> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return await GenerateTokens(user);
        }

        public async Task<AuthResponseDto> Register(RegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email is already in use.");

            var verificationToken = GenerateEmailVerificationToken();

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsEmailVerified = false,
                EmailVerificationToken = verificationToken,
                EmailVerificationTokenExpires = DateTime.UtcNow.AddDays(1)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            await _emailService.SendVerificationEmailAsync(user.Email, verificationToken);

            return new AuthResponseDto
            {
                Message = "Registration successful. Please check your email to verify your account."
            };
        }


        public async Task<TokenResponse> RefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(refreshToken);

            var userId = int.Parse(token.Claims.First(claim => claim.Type == "sub").Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            return await GenerateTokens(user);
        }

        public async Task<bool> RevokeToken(string token)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == token);

            if (user == null)
            {
                return false;   
            }

            user.RefreshToken = null;
            await _context.SaveChangesAsync();
            return true;
        }


        private async Task<TokenResponse> GenerateTokens(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            // Refresh token oluştur
            var refreshToken = GenerateRefreshToken();

            // Refresh token'ı kullanıcıya kaydet
            user.RefreshToken = refreshToken;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new TokenResponse
            {
                Token = jwtToken,
                RefreshToken = refreshToken,
                Expires = tokenDescriptor.Expires.Value
            };
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
