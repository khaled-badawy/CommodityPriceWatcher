namespace Commodity_Prices_Watcher.DAL
{
    public interface ISharedPricesRepo
    {
        List<SharedPrice> GetMyShares(int userId);
        SharedPrice? GetShare(int shareId);
        Task AddShareAsync(SharedPrice sharedPrice);
        void SaveChanges();
    }
}
