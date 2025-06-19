using CodeReviewAPI.DTOs;
using System.Threading.Tasks;

namespace CodeReviewAPI.Services
{
    public interface IAuthService
    {
        Task<TokenResponse> Login(LoginDto loginDto);
        Task<TokenResponse> Register(RegisterDto registerDto);
        Task<TokenResponse> RefreshToken(string refreshToken);
        Task<bool> RevokeToken(string token);
    }
}
