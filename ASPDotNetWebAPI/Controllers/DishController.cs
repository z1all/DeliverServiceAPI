using ASPDotNetWebAPI.CustomValidationAttributes;
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

        [HttpGet]
        public async Task<DishPagedListDTO> GetDishesAsync([FromQuery] DishCategory?[] category, [FromQuery] bool isVegetarian = false, [FromQuery] DishSorting dishSorting = DishSorting.RatingAsc, [FromQuery] int page = 1)
        {
            return await _dishService.GetDishesAsync(category, isVegetarian, dishSorting, page);
        }

        [HttpGet("{id}")]
        public async Task<DishDTO> GetDishInfoAsync(Guid id)
        {
            return await _dishService.GetDishAsync(id);
        }

        [HttpGet("{id}/rating/check")]
        [CustomAuthorize]
        public async Task<bool> CheckSetRating(Guid id)
        {
            var token = JWTTokenService.GetTokenFromHeader(HttpContext);

            return await _dishService.CheckToSetRatingAsync(id, token);
        }

        [HttpPost("{id}/rating")]
        [CustomAuthorize]
        public async Task<DishDTO> SetRating(Guid id, [FromQuery] int value)
        {
            var token = JWTTokenService.GetTokenFromHeader(HttpContext);

            return await _dishService.SetRatingAsync(id, token, value);
        }
    }
}
// Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiI0OTljYTVlOS0zZjRmLTRmMGMtOTllOS03Yjc1NmE4ODE3OTAiLCJKVEkiOiIxODRiNGZiMy01OWVkLTRhM2EtOTg4NS1jZDgzOGI5YjlhZDMiLCJuYmYiOjE2OTgyNTkzMTgsImV4cCI6MTY5ODI2MjkxNywiaWF0IjoxNjk4MjU5MzE4LCJpc3MiOiJISVRzIn0.zJL_O-Dpn-uAMeXSy8nOm2ZEwgi_o4U7i7X4FQzzLac

// 4ee393fc-af18-4636-be23-08dac7a0ede1
