using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [DataType(DataType.Password)]
        public string ReNewPassword { get; set; } = string.Empty;

        [Required]
        public string ActivationCode { get; set; } = string.Empty;


        [Required]
        public string ResetPasswordToken { get; set; } = string.Empty;

    }
}
