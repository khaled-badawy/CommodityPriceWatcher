
using Commodity_Prices_Watcher.DAL;
using Microsoft.IdentityModel.Tokens;

namespace Commodity_Prices_Watcher.BL
{
    public class StaticContentManager : IStaticContentManager
    {
        private readonly IStaticContentRepo _staticContentRepo;

        public StaticContentManager(IStaticContentRepo staticContentRepo)
        {
            _staticContentRepo = staticContentRepo;
        }
        public ReadStaticContetnt? Get(int id)
        {
            var content = _staticContentRepo.Get(id);
            if (content == null) return null;
            return new ReadStaticContetnt
            {
                PageNameArabic = content.TitleA,
                PageNameEnglish = content.PageName,
                Description = content.DescriptionA,
                RouterLink = content.RouterLink,
                Icon = content.Icon
            };
        }
        public List<ReadStaticContetnt> GetAll()
        {
            var contents = _staticContentRepo.GetAll();
            if (contents.IsNullOrEmpty()) return new List<ReadStaticContetnt>();
            return contents.Select(content => new ReadStaticContetnt
            {
                PageNameArabic = content.TitleA,
                PageNameEnglish = content.PageName,
                Description = content.DescriptionA,
                RouterLink = content.RouterLink,
                Icon = content.Icon
            }).ToList();
        }
    }
}
