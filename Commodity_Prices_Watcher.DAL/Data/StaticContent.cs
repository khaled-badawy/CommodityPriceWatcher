namespace Commodity_Prices_Watcher.DAL
{
    public class StaticContent
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string? PageName { get; set; }

        public string? RouterLink { get; set; }

        public string TitleA { get; set; } = null!;

        public string? TitleE { get; set; }

        public string? DescriptionA { get; set; }

        public string? DescriptionE { get; set; }

        public string? Icon { get; set; }

        public int SortIndex { get; set; }

        public bool Active { get; set; }
    }
}
