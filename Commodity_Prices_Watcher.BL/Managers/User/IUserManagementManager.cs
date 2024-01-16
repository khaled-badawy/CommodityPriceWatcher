using Commodity_Prices_Watcher.DAL;
using Microsoft.AspNetCore.Http;

namespace Commodity_Prices_Watcher.BL
{
    public interface IUserManagementManager
    {
        Task<AuthModel> RemoveProfilePicture(ApplicationUser user);
        Task<AuthModel> UpdateProfilePicture(ApplicationUser user , IFormFile newPicture);
        Task<AuthModel> UpdateFullName(ApplicationUser user , string newFullName);
        Task<AuthModel> UpdateEmail(ApplicationUser user , string newEmail);
        Task<AuthModel> UpdatePhoneNumber(ApplicationUser user , string newPhone);
        Task<AuthModel> ConfirmUpdatePhoneNumber(ApplicationUser user , string newPhoneNumber, string activationCode);
        Task<AuthModel> ChangePassword(ApplicationUser user , string password , string newPassword , string reNewPassword);
        Task<ForgetPasswordModel> ForgotPassword(string userPhone);
        Task<AuthModel> ResetPassword(ApplicationUser user , string activationCode, string resetPasswordToken, string newPassword, string reNewPassword);
        Task<AuthModel> NumberToReactivateAccount(string phoneNumber);
        Task<AuthModel> ReactivateAccount(ApplicationUser user, string activationCode);
        Task<AuthModel> DeleteAccount(ApplicationUser user, string userPassword);
        List<ReadSharedPrice> GetMyShares(ApplicationUser user);

        // may to modify some of parameters to meet the intented requirments
    }
}
