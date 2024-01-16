using Commodity_Prices_Watcher.DAL;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Commodity_Prices_Watcher.BL
{
    public class SharedPriceManager : ISharedPriceManager
    {
        private readonly ISharedPricesRepo _sharedPricesRepo;
        private readonly IConfiguration _configuration;
        private string urlUserPicture { get; }
        private string urlSharedPrice { get; }

        public SharedPriceManager(ISharedPricesRepo sharedPricesRepo, IConfiguration configuration)
        {
            _sharedPricesRepo = sharedPricesRepo;
            _configuration = configuration;
            urlUserPicture = $"{_configuration.GetSection("ServerDownloadPath").Value!}/user/Attachment";
            urlSharedPrice = $"{_configuration.GetSection("ServerDownloadPath").Value!}/PriceShare/Attachment";
        }
        public ReadSharedPrice? GetShare(int id)
        {
            var share = _sharedPricesRepo.GetShare(id);
            if (share == null) return null;
            return new ReadSharedPrice
            {
                Id = share.Id,
                Price = share.Price,
                StoreName = share.StoreName,
                CategoryName = share.CommodityCategory.ArabicName,
                Description = share.Description,
                PublishedDate = share.CreateDate.ToString("dd MMMM yyyy", new CultureInfo("ar-AE")),
                UserName = share.ApplicationUser.UserName,
                UserPicture = share.ApplicationUser.ProfilePicture == null ? null : $"{urlUserPicture}/{share.ApplicationUser.Id}/{share.ApplicationUser.ProfilePicture}",
                Attachments = share.AttachmentsLookUp!.Select(x => $"{urlSharedPrice}/{x.Id}/{x.fileName}").ToList(),
            };
        }
        public async Task<int> AddShareAsync(AddSharedPriceDto dto, int userId)
        {
            SharedPrice sharedPriceToAdd = new SharedPrice()
            {
                Price = dto.Price,
                StoreName = dto.StoreName,
                Description = dto.Description,
                CommodityCategoryId = dto.CommodityCategoryId,
                ApplicationUserId = userId,
                Longitude = dto.Longitude,
                Latitude = dto.Latitude,
                CreateDate = DateTime.UtcNow,
                IsActive = true
            };

            await _sharedPricesRepo.AddShareAsync(sharedPriceToAdd);
            _sharedPricesRepo.SaveChanges();
            return sharedPriceToAdd.Id;
        }
    }
}
