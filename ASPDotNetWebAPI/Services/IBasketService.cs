using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IBasketService
    {
        Task<List<DishBasketDTO>> GetDishInBasketAsync(Guid userId);
        Task PutDishInBasketAsync(Guid userId, Guid dishId);
        Task RemoveDishFromBasketAsync(Guid userId, Guid dishId, bool increase);
    }
}
