using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL
{
    public class UpdateFullNameDto
    {
        [Required]
        public string NewFullName { get; set; } = string.Empty;
    }
}
