using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<SearchAddressDTO> GetAddress([FromQuery] int? parentObjectId, [FromQuery] string? query)
        {
            var answer = _addressService.GetChildObjects(parentObjectId != null ? (int)parentObjectId : 0, query);

            //foreach (var number in answer)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine(number.ObjectId);
            //    Console.WriteLine(number.ObjectGuid);
            //    Console.WriteLine(number.Text);
            //    Console.WriteLine(number.ObjectLevel);
            //    Console.WriteLine();
            //}

            return answer;
        }

        [HttpGet("getaddresschain")]
        public IEnumerable<SearchAddressDTO> GetAddressChain([FromQuery] Guid? ObjectGuid)
        {
            throw new NotImplementedException();
        }
    }
}
