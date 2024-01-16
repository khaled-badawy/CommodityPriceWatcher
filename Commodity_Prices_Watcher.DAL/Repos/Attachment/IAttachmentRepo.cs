namespace Commodity_Prices_Watcher.DAL
{
    public interface IAttachmentRepo
    {
        void Add(AttachmentsLookUp attachment);
        void AddMultipleAttachment(List<AttachmentsLookUp> attachments);
        void SaveChanges();
    }
}
