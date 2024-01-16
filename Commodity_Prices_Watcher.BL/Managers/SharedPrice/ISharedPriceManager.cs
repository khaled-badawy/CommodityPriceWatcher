namespace Commodity_Prices_Watcher.BL
{
    public interface ISharedPriceManager
    {
        ReadSharedPrice? GetShare(int id);
        Task<int> AddShareAsync(AddSharedPriceDto dto, int userId);
    }
}
