using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "رقم الهاتف غير صحيح.")]
        public string UserPhone { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور ليستا متماثلتين.")]
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الالكتروني مطلوب")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد البريد الإلكتروني مطلوب")]
        [Compare("Email", ErrorMessage = "البريد الإلكتروني وتأكيد البريد الإلكتروني ليسا متماثلين.")]
        [EmailAddress]
        public string ConfirmEmail { get; set; } = string.Empty;

        public DateTime? Birthdate { get; set; }
    }
}
