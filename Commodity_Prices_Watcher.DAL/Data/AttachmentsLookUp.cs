using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commodity_Prices_Watcher.DAL
{
    [Owned]
    public class AttachmentsLookUp
    {
        public int Id { get; set; }
        public string fileName { get; set; } = string.Empty; 
        public int SharedPriceId { get; set; }
        [ForeignKey("SharedPriceId")]
        public virtual SharedPrice SharedPrice { get; set; } = null!;
    }
}
