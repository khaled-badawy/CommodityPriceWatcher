using Commodity_Prices_Watcher.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Commodity_Prices_Watcher.BL
{
    public class UserManagementManager : IUserManagementManager
    {
        private readonly ISharedPricesRepo _sharedPricesRepo;
        private readonly IEmailManager _emailManager;
        private readonly ISmsManager _smsManager;
        private readonly IConfiguration _configuration;
        private readonly IUploadFileManager _uploadFileManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthManager _authManager;
        private string urlUserPicture { get; }

        public UserManagementManager(ISharedPricesRepo sharedPricesRepo,IEmailManager emailManager,ISmsManager smsManager,IConfiguration configuration,IUploadFileManager uploadFileManager,UserManager<ApplicationUser> userManager, IAuthManager authManager)
        {
            _sharedPricesRepo = sharedPricesRepo;
            _emailManager = emailManager;
            _smsManager = smsManager;
            _configuration = configuration;
            _uploadFileManager = uploadFileManager;
            _userManager = userManager;
            _authManager = authManager;
            urlUserPicture = $"{_configuration.GetSection("ServerDownloadPath").Value!}/user/Attachment";
        }

        public async Task<AuthModel> RemoveProfilePicture(ApplicationUser user)
        {
            var authModel = new AuthModel();
            if (user.ProfilePicture is null)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "لا يوجد صورة ملف شخصي";
                return authModel;
            }

            user.ProfilePicture = null;
            await _userManager.UpdateAsync(user);
            await _authManager.UpdateUserClaim(user, new Claim("UserPicture", "null"));

            var jwtSecurityToken = await _authManager.CreateJwtTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.TokenExpiration = jwtSecurityToken.ValidTo;

            return authModel;
        }
        public async Task<AuthModel> UpdateProfilePicture(ApplicationUser user, IFormFile newPicture)
        {
            var authModel = new AuthModel();

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            bool isImage = allowedExtensions.Contains(Path.GetExtension(newPicture.FileName).ToLower());
            if (!isImage)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "يجب تحديث صورة المستخدم باستخدام ملف خاص بالصور فقط";
                return authModel;
            }

            var uploadResult = await _uploadFileManager.UploadFile(newPicture , "user", user.Id, $"user{user.Id}")!;
            if (uploadResult.IsNullOrEmpty())
            {
                // for testing purpose
                authModel.IsAuthenticated = false;
                authModel.Message = "Upload picture failure";
                return authModel;
            }
            if (user.ProfilePicture is null)
            {
                user.ProfilePicture = uploadResult!;
                await _userManager.UpdateAsync(user);
            }

            await _authManager.UpdateUserClaim(user , new Claim("UserPicture", $"{urlUserPicture}/{user.Id}/{user.ProfilePicture}"));
            var jwtSecurityToken = await _authManager.CreateJwtTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.TokenExpiration = jwtSecurityToken.ValidTo;

            return authModel;
        }
        public async Task<AuthModel> UpdateFullName(ApplicationUser user, string newFullName)
        {
            user.FullName = newFullName;
            await _userManager.UpdateAsync(user);
            await _authManager.UpdateUserClaim(user, new Claim(ClaimTypes.GivenName, newFullName));

            var jwtSecurityToken = await _authManager.CreateJwtTokenAsync(user);
            return new AuthModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                TokenExpiration = jwtSecurityToken.ValidTo,
                IsAuthenticated = true
            };
        }
        public async Task<AuthModel> UpdateEmail(ApplicationUser user, string newEmail)
        {
            user.Email = newEmail;
            await _userManager.UpdateAsync(user);
            await _authManager.UpdateUserClaim(user, new Claim(ClaimTypes.Email, newEmail));

            var jwtSecurityToken = await _authManager.CreateJwtTokenAsync(user);
            return new AuthModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                TokenExpiration = jwtSecurityToken.ValidTo,
                IsAuthenticated = true
            };
        }
        public async Task<AuthModel> UpdatePhoneNumber(ApplicationUser user, string newPhone)
        {
            var authModel = new AuthModel();
            var isExisted = await _userManager.FindByNameAsync(newPhone);
            if (isExisted is not null)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "يوجد مستخدم لدينا بهذا الهاتف بالفعل";
                return authModel;
            }

            var activationCode = new Random().Next(100000, 999999).ToString();
            var isSmsSent = _smsManager.SendSMS(newPhone, $"{activationCode}: كود التفعيل الخاص بكم ");
            await _emailManager.SendEmailAsync(new List<string> { user.Email! }, "Commdoity Prices Watcher", $"{activationCode}: كود التفعيل الخاص بكم ");
            if (!isSmsSent)
            {
                // for testing purpose
                authModel.IsAuthenticated = false;
                authModel.Message = "Sent SMS Activation Code failed";
                return authModel;
            }
            user.ActivationCode = activationCode;
            await _userManager.UpdateAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Message = "برجاء ادخال كود التأكيد المرسل اليكم";
            return authModel;
        }
        public async Task<AuthModel> ConfirmUpdatePhoneNumber(ApplicationUser user, string newPhoneNumber, string activationCode)
        {
            var authModel = new AuthModel();
            if (user.ActivationCode != activationCode)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "كود التفعيل خاطئ";
                return authModel;
            }
            await _userManager.SetUserNameAsync(user,newPhoneNumber);
            user.PhoneNumber = newPhoneNumber;
            user.PhoneNumberConfirmed = true;
            await _userManager.UpdateAsync(user);

            await _authManager.UpdateUserClaim(user, new Claim(ClaimTypes.MobilePhone, newPhoneNumber));
            var jwtSecurityToken = await _authManager.CreateJwtTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Message = "تم تحديث رقم الهاتف بنجاح";
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.TokenExpiration = jwtSecurityToken.ValidTo;

            return authModel;
        }
        public async Task<AuthModel> ChangePassword(ApplicationUser user, string password, string newPassword , string reNewPassword)
        {
            var authModel = new AuthModel();
            var isAuthenticated = await _userManager.CheckPasswordAsync(user, password);
            if (!isAuthenticated)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "كلمة المرور غير صحيحة";
                return authModel;
            }

            if (password == newPassword)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "يرجى تقديم كلمة مرور جديدة بدلاً من القديمة";
                return authModel;
             }
            if (password != reNewPassword)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "كلمة المرور و تأكيد كلمة المرور ليستا متطابقتان";
                return authModel;
            }

            await _userManager.ChangePasswordAsync(user,password,newPassword);

            authModel.IsAuthenticated = true;
            authModel.Message = "تم تغيير كلمة المرور بنجاح";
            return authModel;
        }
        public async Task<ForgetPasswordModel> ForgotPassword(string userPhone)
        {
            var forgetPasswordModel = new ForgetPasswordModel();
            var user = await _userManager.FindByNameAsync(userPhone);
            if (user is null)
            {
                forgetPasswordModel.IsAuthenticated = false;
                forgetPasswordModel.Message = "لا يوجد مستخدم لدينا بهذا رقم الهاتف";
                return forgetPasswordModel;
            }

            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                forgetPasswordModel.IsAuthenticated = false;
                forgetPasswordModel.Message = "تم تعليق هذا الحساب برجاء المحاوله مرة أخرى";
                return forgetPasswordModel;
            }

            if (user.IsDeleted == true)
            {
                forgetPasswordModel.IsAuthenticated = false;
                forgetPasswordModel.Message = "تم مسح هذا المستخدم من قبل";
                return forgetPasswordModel;
            }

            var activationCode = new Random().Next(100000, 999999).ToString();
            var isSmsSent = _smsManager.SendSMS(userPhone, $"{activationCode}: كود التفعيل الخاص بكم ");
            if (!isSmsSent)
            {
                // for testing purpose
                forgetPasswordModel.IsAuthenticated = false;
                forgetPasswordModel.Message = "Sending SMS is failed";
                return forgetPasswordModel;
            }
            await _emailManager.SendEmailAsync(new List<string> { user.Email! }, "Commdoity Prices Watcher", $"{activationCode}: كود التفعيل الخاص بكم ");
            user.ActivationCode = activationCode;
            await _userManager.UpdateAsync(user);

            var jwtSecurityToken = await _authManager.CreateJwtTokenAsync(user);
            var phoneToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var phoneTokenString = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(phoneToken));

            forgetPasswordModel.IsAuthenticated = true;
            forgetPasswordModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            forgetPasswordModel.TokenExpiration = jwtSecurityToken.ValidTo;
            forgetPasswordModel.ResetPasswordToken = phoneTokenString;

            return forgetPasswordModel;
        }
        public async Task<AuthModel> ResetPassword(ApplicationUser user, string activationCode , string resetPasswordToken , string newPassword , string reNewPassword)
        {
            var authModel = new AuthModel();
            if (user.ActivationCode != activationCode)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "رمز التفعيل خاطئ";
                return authModel;
            }

            if (newPassword != reNewPassword)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "برجاء ادخال كلمة المرور و تأكيد كلمة المرور متطابقتان";
                return authModel;
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordToken));
            await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            authModel.IsAuthenticated = true;
            authModel.Message = "تم إعادة تعيين كلمة المرور بنجاح";
            return authModel;
        }
        public async Task<AuthModel> DeleteAccount(ApplicationUser user, string userPassword)
        {
            var authModel = new AuthModel();
            var isAuthenticated = await _userManager.CheckPasswordAsync(user, userPassword);
            if (!isAuthenticated)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "رمز مرور خاطئ";
                return authModel;
            }
            user.DeletedDate = DateTime.Now;
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Message = "تم حذف المستخدم بنجاح";
            return authModel;
        }
        public async Task<AuthModel> NumberToReactivateAccount(string phoneNumber)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByNameAsync(phoneNumber);
            if (user == null)
            {
                authModel.IsAuthenticated= false;
                authModel.Message = "لا يوجد مستخدم لدينا بهذا رقم الهاتف";
                return authModel;
            }
            if (user.IsDeleted == false && user.PhoneNumberConfirmed == false)
            {
                var activationCode = new Random().Next(100000, 999999).ToString();
                var isSmsSent =  _smsManager.SendSMS(phoneNumber, $"{activationCode}: كود التفعيل الخاص بكم ");
                if (!isSmsSent)
                {
                    // for testing purpose
                    authModel.IsAuthenticated = false;
                    authModel.Message = "Send SMS message error !";
                    return authModel;
                }
                var isEmailSent = await _emailManager.SendEmailAsync(new List<string> { user.Email! }, "Commdoity Prices Watcher", $"{activationCode}: كود التفعيل الخاص بكم ");
                if (!isEmailSent)
                {
                    authModel.Message = "Send Email is falied";
                    return authModel;
                }
                user.ActivationCode = activationCode;
                await _userManager.UpdateAsync(user);
                var jwtSecurityToken = await _authManager.CreateJwtTokenAsync(user);

                authModel.IsAuthenticated = true;
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.TokenExpiration = jwtSecurityToken.ValidTo;

                return authModel;
            }
            if (user.IsDeleted == true && user.PhoneNumberConfirmed == true)
            {
                TimeSpan? timeOfDeletion = user.DeletedDate - DateTime.UtcNow;
                int daysOfDeletion = timeOfDeletion!.Value.Days;
                if (daysOfDeletion >= 30)
                {
                    authModel.IsAuthenticated = false;
                    authModel.Message = "تم مسح هذا المستخدم برجاء انشاء حساب آخر";
                    return authModel;
                }
                var activationCode = new Random().Next(100000, 999999).ToString();

                _smsManager.SendSMS(phoneNumber, $"{activationCode}:  كود التفعيل الخاص بكم ");
                await _emailManager.SendEmailAsync(new List<string> { user.Email! }, "Commdoity Prices Watcher", $"{activationCode}: كود التفعيل الخاص بكم ");

                user.ActivationCode = activationCode;
                await _userManager.UpdateAsync(user);

                var jwtSecurityToken = await _authManager.CreateJwtTokenAsync(user);

                authModel.IsAuthenticated = true;
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.TokenExpiration = jwtSecurityToken.ValidTo;

                return authModel;
            }
            authModel.IsAuthenticated = false;
            authModel.Message = "هذا المستخدم مفعل";
            return authModel;
        }
        public async Task<AuthModel> ReactivateAccount(ApplicationUser user, string activationCode)
        {
            var authModel = new AuthModel();
            if (user.ActivationCode != activationCode)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "رمز التفعيل خاطئ";
                return authModel;
            }
            user.IsDeleted = false;
            user.PhoneNumberConfirmed = true;

            await _userManager.UpdateAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Message = "تم تفعيل الهاتف بنجاح";
            return authModel;
        }
        public List<ReadSharedPrice> GetMyShares(ApplicationUser user)
        {
            var userShares = _sharedPricesRepo.GetMyShares(user.Id);
            if (userShares.IsNullOrEmpty()) return new List<ReadSharedPrice>();
            return userShares.Select(s => new ReadSharedPrice 
            {
                Id = s.Id,
                Price = s.Price,
                StoreName = s.StoreName,
                Description = s.Description,
                CategoryName = s.CommodityCategory.ArabicName,
                PublishedDate = s.CreateDate.ToString("dd MMMM yyyy", new CultureInfo("ar-AE")),
                UserName = user.UserName,
                UserPicture = s.ApplicationUser.ProfilePicture == null ? null : $"{urlUserPicture}/{user.Id}/{user.ProfilePicture}"
            }).ToList();
        }

    }
}
