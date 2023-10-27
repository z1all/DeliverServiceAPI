using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Get the children of the parent element parentObjectId matching the query string
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<SearchAddressDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<List<SearchAddressDTO>> GetAddress([FromQuery] int? parentObjectId, [FromQuery] string? query)
        {
            return await _addressService.GetChildObjectsAsync(parentObjectId != null ? (int)parentObjectId : 0, query);
        }

        /// <summary>
        /// Get a chain of elements from the root to an object with objectGuid
        /// </summary>
        [HttpGet("getaddresschain")]
        [ProducesResponseType(typeof(List<SearchAddressDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SearchAddressDTO>>> GetAddressChain([FromQuery] Guid ObjectGuid)
        {
            return await _addressService.GetPathFromRootToObjectAsync(ObjectGuid);
        }
    }
}
