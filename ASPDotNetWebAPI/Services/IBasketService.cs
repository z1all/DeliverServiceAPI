using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IBasketService
    {
        Task<List<DishBasketDTO>> GetDishInBasket(Guid userId);
        Task PutDishInBasket(Guid userId, Guid dishId);
        Task RemoveDishFromBasket(Guid userId, Guid dishId, bool increase);
    }
}
