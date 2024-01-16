namespace Commodity_Prices_Watcher.BL
{
    public interface IAttachmentManager
    {
        int Add(string fileName , int sharedPriceId);
        List<int> AddMultipleAttachment(List<string> filesName , int sharedPriceId);
    }
}
