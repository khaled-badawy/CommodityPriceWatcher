using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL
{
    public class UpdatePhoneNumberDto
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "رقم الهاتف غير صحيح.")]
        public string NewUserPhone { get; set; } = string.Empty;
    }
}
