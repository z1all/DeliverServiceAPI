using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Services
{
    public interface IDishService
    {
        Task<DishPagedListDTO> GetDishesAsync(DishCategory?[] category, bool isVegetarian, DishSorting dishSorting, int page);
        Task<DishDTO> GetDishAsync(Guid dishId);
        Task<bool> CheckToSetRatingAsync(Guid dishId, Guid userId);
        Task<DishDTO> SetRatingAsync(Guid dishId, Guid userId, int ratingScore);
    }
}
