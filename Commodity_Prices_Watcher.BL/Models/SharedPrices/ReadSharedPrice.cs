namespace Commodity_Prices_Watcher.BL
{
    public class ReadSharedPrice
    {
        public int Id { get; set; }
        public string? UserName { get; set; } 
        public string? UserPicture { get; set; }
        public string? PublishedDate { get; set; }
        public string? CategoryName { get; set; } 
        public string? StoreName { get; set; } 
        public string? Description { get; set; }
        public float? Price { get; set; }
        public List<string>? Attachments { get; set; }
    }
}
