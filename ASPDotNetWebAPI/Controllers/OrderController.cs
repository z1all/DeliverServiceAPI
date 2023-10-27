using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet("{id}")]
        [CustomAuthorize]
        public async Task<OrderDTO> GetOrderInfo(Guid id)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            throw new NotImplementedException();
        }

        [HttpGet]
        [CustomAuthorize]
        public async Task<List<OrderInfoDTO>> GetListOrderInfo()
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            throw new NotImplementedException();
        }

        [HttpPost]
        [CustomAuthorize]
        public async Task CreatOrderFromBasket()
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            throw new NotImplementedException();
        }

        [HttpPost("{id}/status")]
        [CustomAuthorize]
        public async Task CreateOrderFromBasket(Guid id)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            throw new NotImplementedException();
        }
    }
}
