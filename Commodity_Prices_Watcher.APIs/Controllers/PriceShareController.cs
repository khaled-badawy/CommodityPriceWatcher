using Commodity_Prices_Watcher.BL;
using Commodity_Prices_Watcher.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Commodity_Prices_Watcher.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceShareController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUploadFileManager _uploadFileManager;
        private readonly ISharedPriceManager _sharedPriceManager;
        private readonly IAttachmentManager _attachmentManager;

        public PriceShareController(UserManager<ApplicationUser> userManager,IUploadFileManager uploadFileManager,ISharedPriceManager sharedPriceManager , IAttachmentManager attachmentManager)
        {
            _userManager = userManager;
            _uploadFileManager = uploadFileManager;
            _sharedPriceManager = sharedPriceManager;
            _attachmentManager = attachmentManager;
        }

        #region Add Price

        [HttpPost]
        [Authorize]
        [Route("AddPrice")]

        public async Task<ActionResult> AddPrice([FromForm(Name =("share"))]AddSharedPriceDto dto,[FromForm(Name = "file")] List<IFormFile> pictures)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }
            var sharedPriceId = await _sharedPriceManager.AddShareAsync(dto,user.Id);
            if (sharedPriceId == 0) return BadRequest();
            var newNames = new List<string>();
            foreach (var picture in pictures)
            {
                var newName = _uploadFileManager.RenameFile(picture, $"sharedPrice{sharedPriceId}");
                newNames.Add(newName);
               // var attachmentId = _attachmentManager.Add(newName, sharedPriceId);
               // await _uploadFileManager.UploadFile(picture, "PriceShare", attachmentId, $"sharedPrice{sharedPriceId}")!;
            }
            var attachmentsId = _attachmentManager.AddMultipleAttachment(newNames, sharedPriceId);
            var zippedList = pictures.Zip(attachmentsId ,(pic , attachmentId) => new { picture = pic , attachId = attachmentId });
            foreach (var item in zippedList)
            {
                await _uploadFileManager.UploadFile(item.picture, "PriceShare", item.attachId, $"sharedPrice{sharedPriceId}")!;
            }
            return Ok(new {message = "تمت المشاركة بنجاح"});
        }

        #endregion

        #region Get Shared Price by Id

        [HttpGet]
        [Route("GetPrice")]

        public ActionResult<ReadSharedPrice> GetById(int priceId)
        {
            var sharedPrice = _sharedPriceManager.GetShare(priceId);
            if (sharedPrice == null) return NotFound();
            return Ok(sharedPrice);
        }

        #endregion

    }
}
