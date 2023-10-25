using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Services
{
    public interface IDishService
    {
        Task<DishPagedListDTO> GetDishes(DishCategory? [] category, bool isVegetarian, DishSorting dishSorting, int page);
        Task<DishDTO> GetDish(Guid id);
        Task<bool> CheckToSetRating(Guid id, string token);
        Task SetRating(Guid id, string token, int ratingScore);
    }
}
