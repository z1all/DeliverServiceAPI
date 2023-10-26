using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ASPDotNetWebAPI.Exceptions;
using ASPDotNetWebAPI.Helpers;

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

            var dishes = await _dbContext.Dishes
                .Where(dish => (isVegetarian == true ? dish.IsVegetairian : true) && (category.IsNullOrEmpty() ? true : category.Contains(dish.Category))).
                Select(dish => new DishDTO()
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

            dishes = SortDishes(dishSorting, dishes);

            var countOfDishOnPageStr = _configuration.GetValue<string>("ApiSettings:CountOfDishOnPage");
            int countOfDishOnPage = countOfDishOnPageStr != null ? int.Parse(countOfDishOnPageStr) : 5;
            int countOfPages = (int)Math.Ceiling((double)dishes.Count() / countOfDishOnPage);

            if (countOfPages < page)
            {
                throw new BadRequestException($"Invalid value for attribute page. The number of pages {countOfPages} is less than the requested page {page}.");
            }

            dishes = dishes.Skip(countOfDishOnPage * (page - 1)).Take(countOfDishOnPage).ToList();

            return new DishPagedListDTO()
            {
                Dishes = dishes,
                Pagination = new PageInfoDTO()
                {
                    Size = dishes.Count(),
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

            var dishInCarts = await
                _dbContext.DishInCarts
                .FirstOrDefaultAsync(dishInCarts => dishInCarts.UserId == userId && dishInCarts.DishId == dishId && dishInCarts.OrderId != null);

            if (dishInCarts == null)
            {
                return false;
            }

            return true;
        }

        public async Task<DishDTO> SetRatingAsync(Guid dishId, Guid userId, int ratingScore)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);
            if (dish == null)
            {
                throw new NotFoundException($"Dish with Guid {dishId} not found.");
            }

            var user = await _dbContext.Users.FirstAsync(user => user.Id == userId);

            if (!await CheckToSetRatingAsync(dishId, userId))
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

        private List<DishDTO> SortDishes(DishSorting dishSorting, List<DishDTO> dishes)
        {
            switch (dishSorting)
            {
                case DishSorting.NameAsc:
                    dishes = dishes.OrderBy(dish => dish.Name).ToList();
                    break;
                case DishSorting.NameDesc:
                    dishes = dishes.OrderByDescending(dish => dish.Name).ToList();
                    break;
                case DishSorting.PriceAsc:
                    dishes = dishes.OrderBy(dish => dish.Price).ToList();
                    break;
                case DishSorting.PriceDesc:
                    dishes = dishes.OrderByDescending(dish => dish.Price).ToList();
                    break;
                case DishSorting.RatingAsc:
                    dishes = dishes.OrderBy(dish => dish.Rating).ToList();
                    break;
                case DishSorting.RatingDesc:
                    dishes = dishes.OrderByDescending(dish => dish.Rating).ToList();
                    break;
            }

            return dishes;
        }
    }
}
