using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _dbContext;

        public BasketService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public Task<List<DishBasketDTO>> GetDishInBasket(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task PutDishInBasket(Guid userId, Guid dishId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveDishFromBasket(Guid userId, Guid dishId, bool increase)
        {
            throw new NotImplementedException();
        }
    }
}
