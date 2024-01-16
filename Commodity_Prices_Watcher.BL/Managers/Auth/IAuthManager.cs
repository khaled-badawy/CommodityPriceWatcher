using Commodity_Prices_Watcher.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Commodity_Prices_Watcher.BL
{
    public interface IAuthManager
    {
        Task<AuthModel> RegisterAsync(RegisterDto registerDto , IFormFile? ProfilePicture);
        Task<AuthModel> LoginAsync(LoginDto loginDto); // Login
        Task<IdentityResult> UpdateUserClaim(ApplicationUser user, Claim newClaim);
        Task<AuthModel> GenerateNewRefreshTokenAsync(string refreshToken);
        Task<AuthModel> GenerateNewJwtTokenAsync(string refreshToken);
        Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
