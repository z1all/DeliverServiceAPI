using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [CustomAuthorize]
        public async Task<OrderDTO> GetOrderInfo(Guid id)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            return await _orderService.GetOrderInfoAsync(userId, id);
        }

        [HttpGet]
        [CustomAuthorize]
        public async Task<List<OrderInfoDTO>> GetListOrderInfo()
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            return await _orderService.GetOrderInfoListAsync(userId);
        }

        [HttpPost]
        [CustomAuthorize]
        public async Task CreatOrderFromBasket([FromBody]OrderCreateDTO orderCreateDTO)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _orderService.CreateOrderFormBasketAsync(userId, orderCreateDTO);
        }

        [HttpPost("{id}/status")]
        [CustomAuthorize]
        public async Task ConfirmOrderDelivery(Guid id)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _orderService.ConfirmOrderDeliveryAsync(userId, id);
        }
    }
}
