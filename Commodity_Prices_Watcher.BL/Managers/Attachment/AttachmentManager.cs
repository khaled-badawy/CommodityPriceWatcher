using Commodity_Prices_Watcher.DAL;

namespace Commodity_Prices_Watcher.BL
{
    public class AttachmentManager : IAttachmentManager
    {
        private readonly IAttachmentRepo _attachmentRepo;

        public AttachmentManager(IAttachmentRepo attachmentRepo)
        {
            _attachmentRepo = attachmentRepo;
        }
        public int Add(string newFileName , int sharedPriceId)
        {
            var attachment = new AttachmentsLookUp()
            {
                SharedPriceId = sharedPriceId,
                fileName = newFileName,
            };
            _attachmentRepo.Add(attachment);
            _attachmentRepo.SaveChanges();
            return attachment.Id;
        }

        public List<int> AddMultipleAttachment(List<string> filesName, int sharedPriceId)
        {
            var attachmentsToAdd = new List<AttachmentsLookUp>();
            foreach (var file in filesName)
            {
                var attachment = new AttachmentsLookUp()
                {
                    SharedPriceId = sharedPriceId,
                    fileName = file
                };
                attachmentsToAdd.Add(attachment);
            }
            _attachmentRepo.AddMultipleAttachment(attachmentsToAdd);
            _attachmentRepo.SaveChanges();
            return attachmentsToAdd.Select(attachment => attachment.Id).ToList();
        }
    }
}
