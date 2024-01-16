using Commodity_Prices_Watcher.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Commodity_Prices_Watcher.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityCategoryController : ControllerBase
    {
        private readonly ICommodityCategoryManager _commodityCategoryManager;

        public CommodityCategoryController(ICommodityCategoryManager commodityCategoryManager)
        {
            _commodityCategoryManager = commodityCategoryManager;
        }

        [HttpGet]
        [Route("GetAll")]

        public ActionResult<List<ReadCommodityCategory>> GetAll()
        {
            var categories = _commodityCategoryManager.GetCategories();
            if (categories.IsNullOrEmpty()) return NotFound(new {message = "لا يوجد"});
            return Ok(categories);
        }
    }
}
