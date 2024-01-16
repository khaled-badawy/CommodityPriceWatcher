namespace Commodity_Prices_Watcher.DAL
{
    public class JWT
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int DurationInHours { get; set; }
    }
}
