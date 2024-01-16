using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commodity_Prices_Watcher.DAL
{
    public class SharedPrice
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CommodityCategoryId { get; set; }
        public int ApplicationUserId { get; set; }
        public string? Description { get; set; }
        public string StoreName { get; set; } = string.Empty; 
        public float Price { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
       // public Point? Shape { get; set; } 
        public bool IsActive { get; set; }
        public int? SortIndex { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("CommodityCategoryId")]
        public virtual CommodityCategory CommodityCategory { get; set; } = null!;
        public List<AttachmentsLookUp>? AttachmentsLookUp { get; set; } 
    }
}
