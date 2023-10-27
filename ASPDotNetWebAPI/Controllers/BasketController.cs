using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models;
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

        [HttpGet]
        [CustomAuthorize]
        public async Task<List<DishBasketDTO>> GetDishInBasket()
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            var dishes = await _basketService.GetDishInBasketAsync(userId);

            return dishes;
        }


        [HttpPost("/dish/{dishId}")]
        [CustomAuthorize]
        public async Task PutDishInBasket(Guid dishId)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _basketService.PutDishInBasketAsync(userId, dishId);
        }

        [HttpDelete("/dish/{dishId}")]
        [CustomAuthorize]
        public async Task DeletDishFromBasket(Guid dishId, [FromQuery]bool increase = false)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _basketService.RemoveDishFromBasketAsync(userId, dishId, increase);          
        }

        // Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIzODhhYzM2MC00Y2Y0LTRmNDQtYjE5My0zN2I4NjY5NzA4OGQiLCJKVEkiOiI2MTQ4MWE1Yi02YzdjLTQzOWEtYThiNi1jYzFmMDRiN2FkZGEiLCJuYmYiOjE2OTg0MDEwOTUsImV4cCI6MTY5ODQwNDY5NSwiaWF0IjoxNjk4NDAxMDk1LCJpc3MiOiJISVRzIn0.pcH8wJ9rXpXnzz-t2PNssWNxx0H-C1trlF66CSJxuAM
    }
}
