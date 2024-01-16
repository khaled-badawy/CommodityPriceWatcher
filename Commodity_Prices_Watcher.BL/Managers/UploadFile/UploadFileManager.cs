using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Commodity_Prices_Watcher.BL
{
    public class UploadFileManager : IUploadFileManager
    {
        private readonly IConfiguration _configuration;
        private string urlUpload { get; }
        public UploadFileManager(IConfiguration configuration)
        {
            _configuration = configuration;
            urlUpload = $"{_configuration.GetSection("ServerUploadPath").Value!}";
        }

        public async Task<string>? UploadFile(IFormFile formFile, string entityType, int entityId, string newFileName)
        {
            var renamedFile = RenameFile(formFile, newFileName);
            var filePath = GetFilePath(renamedFile, entityType, entityId);
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                return string.Empty;
                throw; // for testing purpose
            }

            return renamedFile;
        }

        private string GetStaticContent(string entityType, int id)
        {
            string result = "";

            switch (entityType)
            {
                case "user":
                    result = Path.Combine($"{urlUpload}\\user\\Attachment\\{id}");
                    break;

                case "PriceShare":
                    result = Path.Combine($"{urlUpload}\\PriceShare\\Attachment\\{id}");
                    break;
            }
            try
            {
                if (!Directory.Exists(result))
                {
                    try
                    {
                        Directory.CreateDirectory(result);
                    }
                    catch (Exception ex)
                    {
                        using (StreamWriter writer = System.IO.File.AppendText($"{urlUpload}\\errors.txt"))
                        {
                            writer.WriteLine("error create directory \n" + $"{ex.Message}");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                using (StreamWriter writer = System.IO.File.AppendText($"{urlUpload}\\errors.txt"))
                {
                    writer.WriteLine("error existing directory \n" + $"{ex.Message}");
                }
            }
            return result;
        }
        private string GetFilePath(string FileName, string entityType, int id)
        {
            return Path.Combine(GetStaticContent(entityType, id), FileName);
        }
        public string RenameFile(IFormFile _iFormFile, string newFileName)
        {
            FileInfo fileInfo = new FileInfo(_iFormFile.FileName);
            return $"{newFileName}{fileInfo.Extension}";
        }
    }
}
