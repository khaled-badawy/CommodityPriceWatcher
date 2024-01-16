
namespace Commodity_Prices_Watcher.DAL
{
    public class StaticContentRepo : IStaticContentRepo
    {
        private readonly CommodityPricesWatcherContext _context;

        public StaticContentRepo(CommodityPricesWatcherContext context)
        {
            _context = context;
        }

        public StaticContent? Get(int id)
        {
            return _context.StaticContent.Find(id);
        }

        public IQueryable<StaticContent> GetAll()
        {
            return _context.Set<StaticContent>().Where(s => s.Active == true).OrderBy(s => s.SortIndex);
        }
    }
}
