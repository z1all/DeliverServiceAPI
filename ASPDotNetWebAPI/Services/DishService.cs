using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ASPDotNetWebAPI.Exceptions;

namespace ASPDotNetWebAPI.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public DishService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<DishPagedListDTO> GetDishesAsync(DishCategory?[] category, bool isVegetarian, DishSorting dishSorting, int page)
        {
            if (page < 1)
            {
                throw new BadRequestException($"Invalid value for attribute page. The page number cannot be less than 1. Your number: {page}.");
            }

            var dishesRequest = _dbContext.Dishes
                .Where(dish => (isVegetarian == true ? dish.IsVegetairian : true) && (category.IsNullOrEmpty() ? true : category.Contains(dish.Category)));

            dishesRequest = SortDishes(dishSorting, dishesRequest);

            var countOfDishOnPageStr = _configuration.GetValue<string>("GlobalConstant:CountOfDishOnPage");
            int countOfDishOnPage = countOfDishOnPageStr != null ? int.Parse(countOfDishOnPageStr) : 5;
            int countOfDishes = await dishesRequest.CountAsync();
            int countOfPages = (int)Math.Ceiling((double)countOfDishes / countOfDishOnPage);

            if (countOfPages < page)
            {
                throw new BadRequestException($"Invalid value for attribute page. The number of pages {countOfPages} is less than the requested page {page}.");
            }

            var listOfDishes = await dishesRequest
                .Skip(countOfDishOnPage * (page - 1)).Take(countOfDishOnPage)
                .Select(dish => new DishDTO()
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    Price = dish.Price,
                    Image = dish.Image,
                    IsVegetairian = dish.IsVegetairian,
                    Rating = dish.Rating,
                    Category = dish.Category
                })
                .ToListAsync();

            return new DishPagedListDTO()
            {
                Dishes = listOfDishes,
                Pagination = new PageInfoDTO()
                {
                    Size = countOfDishes,
                    Count = countOfPages,
                    Current = page,
                }
            };
        }

        public async Task<DishDTO> GetDishAsync(Guid dishId)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);

            if (dish == null)
            {
                throw new NotFoundException($"Dish with Guid {dishId} not found.");
            }

            return new DishDTO()
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Image = dish.Image,
                IsVegetairian = dish.IsVegetairian,
                Rating = dish.Rating,
                Category = dish.Category
            };
        }

        public async Task<bool> CheckToSetRatingAsync(Guid dishId, Guid userId)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);
            if (dish == null)
            {
                throw new NotFoundException($"Dish with Guid {dishId} not found.");
            }

            return await CheckToSetRatingWithoutCheckDishAsync(dishId, userId);
        }

        public async Task<DishDTO> SetRatingAsync(Guid dishId, Guid userId, int ratingScore)
        {
            if (ratingScore < 0 || ratingScore > 10)
            {
                throw new BadRequestException("The score must belong to the range from 0 to 10.");
            }

            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);
            if (dish == null)
            {
                throw new NotFoundException($"Dish with Guid {dishId} not found.");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found.");
            }

            if (!await CheckToSetRatingWithoutCheckDishAsync(dishId, userId))
            {
                throw new BadRequestException($"You can't rate a dish with an Guid {dishId}. To do this, you need to buy this dish.");
            }

            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(rating => rating.UserId == userId && rating.DishId == dishId);
            if (rating == null)
            {
                var ratingModel = new Rating()
                {
                    Value = ratingScore,
                    Dish = dish,
                    User = user
                };
                await _dbContext.AddAsync(ratingModel);
            }
            else
            {
                rating.Value = ratingScore;
            }

            await _dbContext.SaveChangesAsync();
            await UpdateRatingAsync(dishId);

            return new DishDTO()
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Image = dish.Image,
                IsVegetairian = dish.IsVegetairian,
                Rating = dish.Rating,
                Category = dish.Category
            };
        }

        private async Task<bool> CheckToSetRatingWithoutCheckDishAsync(Guid dishId, Guid userId)
        {
            var dishInCarts = await _dbContext.DishInCarts
                .Include(dishInCarts => dishInCarts.Order)
                .FirstOrDefaultAsync(dishInCarts => dishInCarts.UserId == userId && dishInCarts.DishId == dishId && dishInCarts.OrderId != null);

            if (dishInCarts == null || (dishInCarts.Order != null && dishInCarts.Order.Status == Status.Delivered || true))
            {
                return false;
            }

            return true;
        }

        private async Task UpdateRatingAsync(Guid dishId)
        {
            var ratings = _dbContext.Ratings.Where(rating => rating.DishId == dishId);
            double sumRatings = await ratings.SumAsync(rating => rating.Value);
            int countRatings = await ratings.CountAsync();

            decimal? averageRating = (decimal?)(sumRatings / countRatings);

            var dish = await _dbContext.Dishes.FirstAsync(dish => dish.Id == dishId);
            dish.Rating = averageRating;

            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<Dish> SortDishes(DishSorting dishSorting, IQueryable<Dish> dishes)
        {
            switch (dishSorting)
            {
                case DishSorting.NameAsc:
                    return dishes.OrderBy(dish => dish.Name);
                case DishSorting.NameDesc:
                    return dishes.OrderByDescending(dish => dish.Name);
                case DishSorting.PriceAsc:
                    return dishes.OrderBy(dish => dish.Price);
                case DishSorting.PriceDesc:
                    return dishes.OrderByDescending(dish => dish.Price);
                case DishSorting.RatingAsc:
                    return dishes.OrderBy(dish => dish.Rating);
                case DishSorting.RatingDesc:
                    return dishes.OrderByDescending(dish => dish.Rating);
            }

            return dishes;
        }
    }
}
