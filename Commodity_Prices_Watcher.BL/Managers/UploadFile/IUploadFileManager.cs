using Microsoft.AspNetCore.Http;

namespace Commodity_Prices_Watcher.BL
{
    public interface IUploadFileManager
    {
        Task<string>? UploadFile(IFormFile formFile , string entityType , int entityId , string newFileName );
        string RenameFile(IFormFile _iFormFile, string newFileName);
    }
}
