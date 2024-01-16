using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL
{
    public class DeleteAccountDto
    {
        [Required]
        public string UserPassword { get; set; } = string.Empty;
    }
}
