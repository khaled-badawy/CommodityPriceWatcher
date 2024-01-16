namespace Commodity_Prices_Watcher.BL
{
    public interface ISmsManager
    {
        bool SendSMS(string phoneNumber, string textMsg);
    }
}
