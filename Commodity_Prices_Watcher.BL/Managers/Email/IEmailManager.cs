namespace Commodity_Prices_Watcher.BL
{
    public interface IEmailManager
    {
        Task<bool> SendEmailAsync(List<string> recieversEmail, string subject, string message);
    }
}
