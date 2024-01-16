using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL
{
    public class UpdateEmailDto
    {
        [Required(ErrorMessage = "البريد الالكتروني مطلوب")]
        [EmailAddress]
        public string NewEmail { get; set; } = string.Empty;
    }
}
