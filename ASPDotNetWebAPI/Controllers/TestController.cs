using ASPDotNetWebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ErrorDTO> Get()
        {
            throw new Exception("BOOM");
        }
    }
}
