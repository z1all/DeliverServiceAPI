using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("search")]
        public async Task<List<SearchAddressDTO>> GetAddress([FromQuery] int? parentObjectId, [FromQuery] string? query)
        {
            var answer = await _addressService.GetChildObjectsAsync(parentObjectId != null ? (int)parentObjectId : 0, query);

            return answer;
        }

        [HttpGet("getaddresschain")]
        public async Task<ActionResult<List<SearchAddressDTO>>> GetAddressChain([FromQuery] Guid ObjectGuid)
        {
            var answer = await _addressService.GetPathFromRootToObjectAsync(ObjectGuid);

            if (answer.IsNullOrEmpty())
            {
                return NotFound(new ResponseDTO()
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = $"Not found object with ObjectGuid={ObjectGuid}"
                });
            }

            return answer;
        }
    }
}
