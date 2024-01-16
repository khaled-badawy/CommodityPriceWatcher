using Commodity_Prices_Watcher.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Commodity_Prices_Watcher.BL
{
    public class AuthManager : IAuthManager
    {
        private readonly IEmailManager _emailManager;
        private readonly IUploadFileManager _uploadFileManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ISmsManager _smsManager;
        private readonly JWT _jwt;

        private string urlUserPicture { get; }

        public AuthManager(IEmailManager emailManager,IUploadFileManager uploadFileManager,UserManager<ApplicationUser> userManager , IConfiguration config , IOptions<JWT> jwt , ISmsManager smsManager)
        {
            _emailManager = emailManager;
            _uploadFileManager = uploadFileManager;
            _userManager = userManager;
            _config = config;
            _smsManager = smsManager;
            _jwt = jwt.Value;
            urlUserPicture = $"{_config.GetSection("ServerDownloadPath").Value!}/user/Attachment";
        }
        public async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {
            var claimsList = await _userManager.GetClaimsAsync(user);

            if (claimsList.IsNullOrEmpty())
            {
                if (user.ProfilePicture is not null)
                {
                    claimsList = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim(ClaimTypes.MobilePhone , user.UserName!),
                        new Claim(ClaimTypes.GivenName , user.FullName!),
                        new Claim("UserPicture", $"{urlUserPicture}/{user.Id}/{user.ProfilePicture}"),
                    };
                }
                else
                {
                    claimsList = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, "User"),
                            new Claim(ClaimTypes.Email, user.Email!),
                            new Claim(ClaimTypes.MobilePhone , user.UserName!),
                            new Claim(ClaimTypes.GivenName , user.FullName!),
                            new Claim("UserPicture", "null"),
                        };
                }

                await _userManager.AddClaimsAsync(user, claimsList);
            }

            //Getting the key ready
            string keyString = _jwt.SecretKey;
            byte[] keyInBytes = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyInBytes);

            //Combine Secret Key with Hashing Algorithm
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //Putting All together
            var expiry = DateTime.Now.AddHours(_jwt.DurationInHours); // for test
            //var expiry = DateTime.Now.AddMonths(_jwt.DurationInHours); // for production
            var jwtSecurityToken = new JwtSecurityToken(
                            issuer: _jwt.Issuer,
                            audience: _jwt.Audience,
                            expires: expiry,
                            claims: claimsList,
                            signingCredentials: signingCredentials
                            );

            return jwtSecurityToken;
        }

        public async Task<AuthModel> GenerateNewRefreshTokenAsync(string refreshToken)
        {
            var authModel = new AuthModel();

            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens!.Any(t => t.Token == refreshToken));
            if (user is null)
            {
                authModel.IsAuthenticated = false; // unnecessary assignment as the default value of bool is false
                authModel.Message = "Invalid Token.";
                return authModel;
            }

            var userRefreshToken = user.RefreshTokens!.Single(t => t.Token == refreshToken);

            if (!userRefreshToken.IsActive)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "Inactive token";

                return authModel;
            }

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens!.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.TokenExpiration = jwtSecurityToken.ValidTo;
            authModel.Username = user.FullName;
            authModel.Email = user.Email;
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }

        public async Task<AuthModel> GenerateNewJwtTokenAsync(string refreshToken)
        {
            var authModel = new AuthModel();

            var user =  _userManager.Users.SingleOrDefault(u => u.RefreshTokens!.Any(t => t.Token == refreshToken));
            if (user is null)
            {
                authModel.IsAuthenticated = false; // unnecessary assignment as the default value of bool is false
                authModel.Message = "Invalid Token.";
                return authModel;
            }

            var userRefreshToken = user.RefreshTokens!.Single(t => t.Token == refreshToken);

            if (!userRefreshToken.IsActive)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "token is expired";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.TokenExpiration = jwtSecurityToken.ValidTo;
            authModel.Username = user.FullName;
            authModel.Email = user.Email;
            authModel.RefreshToken = userRefreshToken.Token;
            authModel.RefreshTokenExpiration = userRefreshToken.ExpiresOn;

            return authModel;
        }

        public async Task<AuthModel> LoginAsync(LoginDto loginDto)
        {
            var authModel = new AuthModel();

            ApplicationUser? user = await _userManager.FindByNameAsync(loginDto.UserPhone);
            if (user is null)
            {
                authModel.Message = "لا يوجد مستخدم لدينا بهذا الهاتف";
                return authModel;
            }
            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "تم تعليق هذا الحساب برجاء المحاوله مرة أخرى";
                return authModel;
            }
            if (user.IsDeleted == true)
            {
                authModel.IsAuthenticated= false;
                authModel.Message = "هذا الحساب تم مسحه من قبل او عدم تفعيله";
                return authModel;
            }
            var isPhoneConfirmed = await _userManager.IsPhoneNumberConfirmedAsync(user);
            if (!isPhoneConfirmed)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "يجب تأكيد تفعيل رقم الهاتف عن طريق كود التفعيل المرسل اليكم";
                return authModel;
            }
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                await _userManager.AccessFailedAsync(user);
                authModel.Message = "كلمة المرور خاطئة";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.TokenExpiration = jwtSecurityToken.ValidTo;
            authModel.Username = user.UserName;
            authModel.Email = user.Email;

            if (user.RefreshTokens!.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens!.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken!.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens!.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return authModel;
        }

        public async Task<AuthModel> RegisterAsync(RegisterDto registerDto , IFormFile? ProfilePicture)
        {
            var authModel = new AuthModel();

            var isPhoneExisted = await _userManager.FindByNameAsync(registerDto.UserPhone);
            if (isPhoneExisted is not null) 
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "هذا الهاتف مسجل لدينا بالفعل";
                return authModel;
            }

            var isEmailExisted = await _userManager.FindByEmailAsync(registerDto.Email);
            if (isEmailExisted is not null)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "هذا البريد الالكتروني مسجل لدينا بالفعل";
                return authModel;
            }

            var activationCode = new Random().Next(100000, 999999).ToString();
            var newUser = new ApplicationUser
            {
                UserName = registerDto.UserPhone,
                PhoneNumber = registerDto.UserPhone,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Birthdate = registerDto.Birthdate,
                IsDeleted = false,
                IsForgotPassEmailConfirmed = false,
                DeletedDate = null,
                CreateDate = DateTime.UtcNow,
                ActivationCode = activationCode
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!createUserResult.Succeeded)
            {
                StringBuilder errorString = new StringBuilder();
                foreach (var error in createUserResult.Errors)
                {
                    errorString.AppendLine(error.Description);
                }
                authModel.IsAuthenticated = false;
                authModel.Message = errorString.ToString();
                return authModel;
            }
            _smsManager.SendSMS(registerDto.UserPhone, $"{activationCode}: كود التفعيل الخاص بكم ");
            await _emailManager.SendEmailAsync(new List<string> { registerDto.Email }, "Commdoity Prices Watcher", $"{activationCode}: كود التفعيل الخاص بكم ");
            if (ProfilePicture is not null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                bool isImage = allowedExtensions.Contains(Path.GetExtension(ProfilePicture.FileName).ToLower());
                if (isImage)
                {
                    var uploadedFileName = await _uploadFileManager.UploadFile(ProfilePicture,"user",newUser.Id,$"user{newUser.Id}")!;
                    if (uploadedFileName.IsNullOrEmpty())
                    {
                        // for testing purpose
                        authModel.IsAuthenticated = false;
                        authModel.Message = "Upload profile picture is failed";
                        return authModel;
                    }
                    newUser.ProfilePicture = uploadedFileName;
                    await _userManager.UpdateAsync(newUser);
                }
            }

            var jwtSecurityToken = await CreateJwtTokenAsync(newUser);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.TokenExpiration = jwtSecurityToken.ValidTo;
            authModel.Username = newUser.UserName;
            authModel.Email = newUser.Email;

            var refreshToken = GenerateRefreshToken();
            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
            newUser.RefreshTokens!.Add(refreshToken);
            await _userManager.UpdateAsync(newUser);

            return authModel;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens!.Any(t => t.Token == refreshToken));

            if (user is null) return false;

            var userRefreshToken = user.RefreshTokens!.Single(t => t.Token == refreshToken);
            if (!userRefreshToken.IsActive) return false;

            userRefreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<IdentityResult> UpdateUserClaim(ApplicationUser user, Claim newClaim)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            Claim userClaim = userClaims.FirstOrDefault(c => c.Type == newClaim.Type)!;

            var removeClaimResult = await _userManager.RemoveClaimAsync(user, userClaim);
            if (!removeClaimResult.Succeeded) return removeClaimResult;

            var addClaimResult = await _userManager.AddClaimAsync(user, newClaim);
            return addClaimResult;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = RandomNumberGenerator.Create();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.Now.AddDays(10),
                CreatedOn = DateTime.Now
            };
        }
    }
}
