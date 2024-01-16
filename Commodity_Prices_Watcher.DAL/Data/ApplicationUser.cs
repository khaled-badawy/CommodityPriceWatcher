using Microsoft.AspNetCore.Identity;

namespace Commodity_Prices_Watcher.DAL
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime? CreateDate { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public string? ActivationCode { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsForgotPassEmailConfirmed { get; set; }
        public DateTime? Birthdate { get; set; }
        public List<SharedPrice>? SharedPrices { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
