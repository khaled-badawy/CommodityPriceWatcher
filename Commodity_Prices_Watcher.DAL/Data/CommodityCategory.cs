namespace Commodity_Prices_Watcher.DAL
{
    public class CommodityCategory
    {
        public int Id { get; set; }
        public string ArabicName { get; set; } = string.Empty;
        public string? EnglishName { get; set; } 
        public bool IsActive { get; set; }
        public int SortIndex { get; set; }
    }
}
