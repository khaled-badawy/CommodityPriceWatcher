using Commodity_Prices_Watcher.BL;
using Commodity_Prices_Watcher.BL.Dtos.User;
using Commodity_Prices_Watcher.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Commodity_Prices_Watcher.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthManager _authManager;
        private readonly IUserManagementManager _userManagementManager;

        public UserController(UserManager<ApplicationUser> userManager,IAuthManager authManager , IUserManagementManager userManagementManager)
        {
            _userManager = userManager;
            _authManager = authManager;
            _userManagementManager = userManagementManager;
        }

        #region Registeration

        [HttpPost]
        [Route("register")]

        public async Task<ActionResult<AuthModel>> Register([FromForm(Name =("user"))] RegisterDto registerDto ,[FromForm(Name ="file")] IFormFile? profilePicture)
        {
            var authModel = await _authManager.RegisterAsync(registerDto, profilePicture);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Login

        [HttpPost]
        [Route("Login")]

        public async Task<ActionResult<AuthModel>> Login(LoginDto loginDto)
        {
            var authModel = await _authManager.LoginAsync(loginDto);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Number To Reactivate Account or to confirm phone number

        [HttpPost]
        [Route("NumberToReactivateOrConfirmPhone")]

        public async Task<ActionResult<AuthModel>> ReactivateOrConfirmPhone(NumberToReactivateAccountDto dto)
        {
            var authModel = await _userManagementManager.NumberToReactivateAccount(dto.UserPhone);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Confirm phone number

        [HttpPost]
        [Authorize]
        [Route("ConfirmPhoneNumber")]

        public async Task<ActionResult<AuthModel>> ConfirmPhoneNumber(ConfirmPhoneNumberDto dto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }
            var authModel = await _userManagementManager.ReactivateAccount(user, dto.ActivationCode);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Delete Account

        [HttpPost]
        [Authorize]
        [Route("DeleteAccount")]

        public async Task<ActionResult<AuthModel>> DeleteAccount(DeleteAccountDto dto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }
            var authModel = await _userManagementManager.DeleteAccount(user, dto.UserPassword);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }
        #endregion

        #region Remove Profile Picture

        [HttpPost]
        [Authorize]
        [Route("RemoveProfilePicture")]

        public async Task<ActionResult<AuthModel>> RemoveProfilePicture()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var authModel = await _userManagementManager.RemoveProfilePicture(user);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Update Profile Picture

        [HttpPost]
        [Authorize]
        [Route("UpdateProfilePicture")]

        public async Task<ActionResult<AuthModel>> UpdateProfilePicture([FromForm(Name ="file")]IFormFile newPicture)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var authModel = await _userManagementManager.UpdateProfilePicture(user , newPicture);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Update Full Name

        [HttpPost]
        [Authorize]
        [Route("UpdateFullName")]

        public async Task<ActionResult<AuthModel>> UpdateFullName(UpdateFullNameDto dto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var authModel = await _userManagementManager.UpdateFullName(user,dto.NewFullName);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Update Email 

        [HttpPost]
        [Authorize]
        [Route("UpdateEmail")]

        public async Task<ActionResult<AuthModel>> UpdateEmail(UpdateEmailDto dto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var authModel = await _userManagementManager.UpdateEmail(user, dto.NewEmail);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Update Phone Number 

        [HttpPost]
        [Authorize]
        [Route("UpdatePhoneNumber")]

        public async Task<ActionResult<AuthModel>> UpdatePhoneNumber(UpdatePhoneNumberDto dto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var authModel = await _userManagementManager.UpdatePhoneNumber(user, dto.NewUserPhone);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Confirm Update Phone Number 

        [HttpPost]
        [Authorize]
        [Route("ConfirmUpdatePhoneNumber")]

        public async Task<ActionResult<AuthModel>> ConfirmUpdatePhoneNumber(ConfirmUpdatePhoneNumberDto dto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var authModel = await _userManagementManager.ConfirmUpdatePhoneNumber(user, dto.NewUserPhone,dto.ActivationCode);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Change Password 

        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]

        public async Task<ActionResult<AuthModel>> ChangePassword(ChangePasswordDto dto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var authModel = await _userManagementManager.ChangePassword(user, dto.OldPassword , dto.NewPassword , dto.ReNewPassword);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Forgot Password 

        [HttpPost]
        [Route("ForgotPassword")]

        public async Task<ActionResult<ForgetPasswordModel>> ForgotPassword(NumberToReactivateAccountDto dto)
        {
            var forgetPasswordModel = await _userManagementManager.ForgotPassword(dto.UserPhone);
            if (!forgetPasswordModel.IsAuthenticated) return BadRequest(forgetPasswordModel);
            return Ok(forgetPasswordModel);
        }

        #endregion

        #region Reset Password 

        [HttpPost]
        [Authorize]
        [Route("ResetPassword")]

        public async Task<ActionResult<AuthModel>> ResetPassword(ResetPasswordDto dto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var authModel = await _userManagementManager.ResetPassword(user,dto.ActivationCode,dto.ResetPasswordToken,dto.NewPassword,dto.ReNewPassword);
            if (!authModel.IsAuthenticated) return BadRequest(authModel);
            return Ok(authModel);
        }

        #endregion

        #region Get Shared Prices

        [HttpGet]
        [Authorize]
        [Route("GetMyShares")]

        public async Task<ActionResult<List<ReadSharedPrice>>> GetMyShares()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }
            var userShares = _userManagementManager.GetMyShares(user);
            if (userShares.IsNullOrEmpty()) return NotFound(new { message = "لا يوجد" });
            return Ok(userShares);
        }

        #endregion

    }
}
