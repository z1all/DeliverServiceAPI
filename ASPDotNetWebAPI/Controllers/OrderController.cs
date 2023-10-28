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

        /// <summary>
        /// Get information about concrete order
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("{id}")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(List<OrderInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<OrderDTO> GetOrderInfo(Guid id)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            return await _orderService.GetOrderInfoAsync(userId, id);
        }

        /// <summary>
        /// Get a list of orders
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [CustomAuthorize]
        [ProducesResponseType(typeof(List<OrderInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<List<OrderInfoDTO>> GetListOrderInfo()
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            return await _orderService.GetOrderInfoListAsync(userId);
        }

        /// <summary>
        /// Creating the order from dishes in basket
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task CreatOrderFromBasket([FromBody]OrderCreateDTO orderCreateDTO)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _orderService.CreateOrderFormBasketAsync(userId, orderCreateDTO);
        }

        /// <summary>
        /// Confirm order delivery
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("{id}/status")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task ConfirmOrderDelivery(Guid id)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _orderService.ConfirmOrderDeliveryAsync(userId, id);
        }
    }
}
