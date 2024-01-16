
using Microsoft.IdentityModel.Tokens;

namespace Commodity_Prices_Watcher.DAL
{
    public class CommodityCategoryRepo : ICommodityCategoryRepo
    {
        private readonly CommodityPricesWatcherContext _context;

        public CommodityCategoryRepo(CommodityPricesWatcherContext context)
        {
            _context = context;
        }
        public List<CommodityCategory> GetCategories()
        {
            var categories = _context.CommodityCategory
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.SortIndex)
                .ToList();
            if (categories.IsNullOrEmpty()) return new List<CommodityCategory> ();
            return categories;
        }
    }
}
