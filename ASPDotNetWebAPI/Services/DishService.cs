using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _dbContext;

        public DishService(ApplicationDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public Task<bool> CheckToSetRating(Guid id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<DishDTO> GetDish(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DishPagedListDTO> GetDishes(DishCategory? category, bool isVegetarian, DishSorting? dishSorting, int page)
        {
            throw new NotImplementedException();
        }

        public Task SetRating(Guid id, string token, int ratingScore)
        {
            throw new NotImplementedException();
        }
    }
}
