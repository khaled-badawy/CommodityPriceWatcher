
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Commodity_Prices_Watcher.DAL
{
    public class SharedPricesRepo : ISharedPricesRepo
    {
        private readonly CommodityPricesWatcherContext _context;

        public SharedPricesRepo(CommodityPricesWatcherContext context)
        {
            _context = context;
        }

        public async Task AddShareAsync(SharedPrice sharedPrice)
        {
            await _context.SharedPrice.AddAsync(sharedPrice);
        }

        public List<SharedPrice> GetMyShares(int userId)
        {
            var userShares = _context.SharedPrice
                .Include(s => s.ApplicationUser)
                .Include(s => s.CommodityCategory)
                .Where(s => s.ApplicationUserId == userId && s.IsActive == true)
                .OrderByDescending(s => s.CreateDate);
            if (userShares.IsNullOrEmpty()) return new List<SharedPrice>();
            return userShares.ToList();
        }

        public SharedPrice? GetShare(int shareId)
        {
            var share = _context.SharedPrice
                .Include(s => s.ApplicationUser)
                .Include(s => s.CommodityCategory)
                .Include(s => s.AttachmentsLookUp)
                .FirstOrDefault(s => s.IsActive == true && s.Id == shareId);
            if (share is null) return null;
            return share;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
