namespace Commodity_Prices_Watcher.BL
{
    public class AddSharedPriceDto
    {
        public int CommodityCategoryId { get; set; }
        public string? Description { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public float Price { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}
