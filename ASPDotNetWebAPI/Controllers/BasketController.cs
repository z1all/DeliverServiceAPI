using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        [HttpGet]
        [CustomAuthorize]
        public async Task<List<DishBasketDTO>> GetDishInBasket()
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            throw new NotImplementedException();
        }


        [HttpPost("/dish/{dishId}")]
        [CustomAuthorize]
        public async Task PutDishInBasket(Guid dishId)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            throw new NotImplementedException();
        }

        [HttpDelete("/dish/{dishId}")]
        [CustomAuthorize]
        public async Task DeletDishFromBasket(Guid dishId, [FromQuery]bool increase = false)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            throw new NotImplementedException();
        }
    }
}
