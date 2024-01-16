using Commodity_Prices_Watcher.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Commodity_Prices_Watcher.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaticContentController : ControllerBase
    {
        private readonly IStaticContentManager _staticContentManager;
        public StaticContentController(IStaticContentManager staticContentManager)
        {
            _staticContentManager = staticContentManager;
        }

        [HttpGet]
        [Route("get_all")]

        public ActionResult<List<ReadStaticContetnt>> GetAll()
        {
            var contents = _staticContentManager.GetAll();
            if (contents.IsNullOrEmpty()) return BadRequest();
            return Ok(contents);
        }
    }
}
