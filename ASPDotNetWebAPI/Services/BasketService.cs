using ASPDotNetWebAPI.Exceptions;
using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Services
{
    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _dbContext;

        public BasketService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DishBasketDTO>> GetDishInBasketAsync(Guid userId)
        {
            var listOfDish = await _dbContext.DishInCarts
                .Where(dishInCart => dishInCart.UserId == userId && dishInCart.OrderId == null)
                .Select(dishInCart => new DishBasketDTO
                {
                    Id = dishInCart.Dish.Id,
                    Name = dishInCart.Dish.Name,
                    Price = dishInCart.Dish.Price,
                    TotalPrice = dishInCart.Dish.Price * dishInCart.Count,
                    Amount = dishInCart.Count,
                    Image = dishInCart.Dish.Image
                })
                .ToListAsync();

            return listOfDish;
        }

        public async Task PutDishInBasketAsync(Guid userId, Guid dishId)
        {
            var dishInCart = await _dbContext.DishInCarts
                .FirstOrDefaultAsync(dishInCart => dishInCart.UserId == userId && dishInCart.DishId == dishId && dishInCart.Order == null);

            if (dishInCart != null)
            {
                dishInCart.Count++;
            }
            else
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with Guid {userId} not found!");
                }

                var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);
                if (dish == null)
                {
                    throw new NotFoundException($"Dish with Guid {dishId} not found!");
                }

                await _dbContext.DishInCarts.AddAsync(new DishInCart()
                {
                    Dish = dish,
                    User = user,
                    Count = 1
                });
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveDishFromBasketAsync(Guid userId, Guid dishId, bool increase)
        {
            var dishInCart = await _dbContext.DishInCarts
                .FirstOrDefaultAsync(dishInCart => dishInCart.UserId == userId && dishInCart.DishId == dishId && dishInCart.Order == null);
            if (dishInCart == null)
            {
                throw new NotFoundException($"Dish with Guid {dishId} not found in the basket");
            }

            if (increase)
            {
                _dbContext.DishInCarts.Remove(dishInCart);
            }
            else
            {
                dishInCart.Count--;
                if (dishInCart.Count < 1)
                {
                    _dbContext.DishInCarts.Remove(dishInCart);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}