using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService) 
        { 
            _dishService = dishService;
        }

        /// <summary>
        /// Get a list of dishes
        /// </summary>   
        [HttpGet]
        [ProducesResponseType(typeof(DishPagedListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<DishPagedListDTO> GetDishesAsync([FromQuery] DishCategory?[] category, [FromQuery] bool isVegetarian = false, [FromQuery] DishSorting dishSorting = DishSorting.RatingAsc, [FromQuery] int page = 1)
        {
            return await _dishService.GetDishesAsync(category, isVegetarian, dishSorting, page);
        }

        /// <summary>
        /// Get information about concrete dish
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DishDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<DishDTO> GetDishInfoAsync(Guid id)
        {
            return await _dishService.GetDishAsync(id);
        }

        /// <summary>
        /// Checks if user is able to ser rating of the dish 
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("{id}/rating/check")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(DishDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<bool> CheckSetRating(Guid id)
        {
            var token = JWTTokenHelper.GetTokenFromHeader(HttpContext);

            return await _dishService.CheckToSetRatingAsync(id, token);
        }

        /// <summary>
        /// Set a rating for a dish
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("{id}/rating")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(DishDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<DishDTO> SetRating(Guid id, [FromQuery] int value)
        {
            var token = JWTTokenHelper.GetTokenFromHeader(HttpContext);

            return await _dishService.SetRatingAsync(id, token, value);
        }
    }
}
