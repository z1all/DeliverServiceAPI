using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        /// <summary>
        /// Get user cart
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [CustomAuthorize]
        [ProducesResponseType(typeof(List<DishBasketDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<List<DishBasketDTO>> GetDishInBasket()
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            var dishes = await _basketService.GetDishInBasketAsync(userId);

            return dishes;
        }

        /// <summary>
        /// Add dish to cart
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("/dish/{dishId}")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task PutDishInBasket(Guid dishId)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _basketService.PutDishInBasketAsync(userId, dishId);
        }

        /// <summary>
        /// Decrease the number of dishes in the cart(if increase = true), or remove the dish completely(increase = false)
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpDelete("/dish/{dishId}")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task DeletDishFromBasket(Guid dishId, [FromQuery] bool increase = false)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _basketService.RemoveDishFromBasketAsync(userId, dishId, increase);
        }
    }
}
