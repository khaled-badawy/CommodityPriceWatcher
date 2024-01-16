
using Commodity_Prices_Watcher.DAL;
using Microsoft.IdentityModel.Tokens;

namespace Commodity_Prices_Watcher.BL
{
    public class CommodityCategoryManager : ICommodityCategoryManager
    {
        private readonly ICommodityCategoryRepo _commodityCategoryRepo;

        public CommodityCategoryManager(ICommodityCategoryRepo commodityCategoryRepo)
        {
            _commodityCategoryRepo = commodityCategoryRepo;
        }
        public List<ReadCommodityCategory> GetCategories()
        {
            var categoriesFromDb = _commodityCategoryRepo.GetCategories();
            if (categoriesFromDb.IsNullOrEmpty()) return new List<ReadCommodityCategory>();
            return categoriesFromDb.Select(c => new ReadCommodityCategory
            {
                Id = c.Id,
                Name = c.ArabicName
            }).ToList();
        }
    }
}
