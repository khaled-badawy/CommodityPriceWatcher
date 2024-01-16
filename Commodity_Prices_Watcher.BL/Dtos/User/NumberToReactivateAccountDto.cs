using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL
{
    public class NumberToReactivateAccountDto
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "رقم الهاتف غير صحيح.")]
        public string UserPhone { get; set; } = string.Empty;
    }
}
