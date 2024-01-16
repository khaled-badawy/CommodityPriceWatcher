

namespace Commodity_Prices_Watcher.DAL
{
    public class AttachmentRepo : IAttachmentRepo
    {
        private readonly CommodityPricesWatcherContext _context;

        public AttachmentRepo(CommodityPricesWatcherContext context)
        {
            _context = context;
        }
        public void Add(AttachmentsLookUp attachment)
        {
             _context.Add(attachment); 
        }

        public void AddMultipleAttachment(List<AttachmentsLookUp> attachments)
        {
            _context.AddRange(attachments);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
