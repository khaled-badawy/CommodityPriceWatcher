using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL.Dtos.User
{
    public class ConfirmPhoneNumberDto
    {
        [Required]
        public string ActivationCode { get; set; } = string.Empty;

    }
}
