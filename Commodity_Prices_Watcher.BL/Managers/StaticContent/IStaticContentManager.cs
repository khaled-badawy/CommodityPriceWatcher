namespace Commodity_Prices_Watcher.BL
{
    public interface IStaticContentManager
    {
        List<ReadStaticContetnt> GetAll();
        ReadStaticContetnt? Get(int id);
    }
}
