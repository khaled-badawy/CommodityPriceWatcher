using Commodity_Prices_Watcher.BL;
using Microsoft.AspNetCore.Mvc;

namespace Commodity_Prices_Watcher.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        #region Generate new refresh token

        [HttpPost]
        [Route("GenerateNewRefreshToken")]

        public async Task<ActionResult<AuthModel>> GenerateNewRefreshToken(RefreshTokenDto dto)
        {
            var authModel = await _authManager.GenerateNewRefreshTokenAsync(dto.RefreshToken);
            return Ok(authModel);
        }

        #endregion

        #region Generate new JWT token

        [HttpPost]
        [Route("GenerateNewJwtToken")]

        public async Task<ActionResult<AuthModel>> GenerateNewJwtToken(RefreshTokenDto dto)
        {
            var authModel = await _authManager.GenerateNewJwtTokenAsync(dto.RefreshToken);
            return Ok(authModel);
        }

        #endregion

        #region Revoke refresh token

        [HttpPost]
        [Route("RevokeToken")]

        public async Task<ActionResult<AuthModel>> RevokeToken(RefreshTokenDto dto)
        {
            var authModel = await _authManager.RevokeRefreshTokenAsync(dto.RefreshToken);
            return Ok(authModel);
        }
        
        #endregion

    }
}
