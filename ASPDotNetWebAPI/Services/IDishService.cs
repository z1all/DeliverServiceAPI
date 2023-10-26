using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Services
{
    public interface IDishService
    {
        Task<DishPagedListDTO> GetDishesAsync(DishCategory? [] category, bool isVegetarian, DishSorting dishSorting, int page);
        Task<DishDTO> GetDishAsync(Guid id);
        Task<bool> CheckToSetRatingAsync(Guid id, string token);
        Task<DishDTO> SetRatingAsync(Guid id, string token, int ratingScore);
    }
}
