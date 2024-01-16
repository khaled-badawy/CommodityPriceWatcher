namespace Commodity_Prices_Watcher.DAL
{
    public interface IStaticContentRepo
    {
        IQueryable<StaticContent> GetAll();
        StaticContent? Get(int id);
    }
}
