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

            SortDishes(dishSorting, dishes);

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
                    Size = countOfDishOnPage,
                    Count = countOfPages,
                    Current = page,
                }
            };
        }

        public async Task<DishDTO> GetDishAsync(Guid id)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == id);

            if (dish == null)
            {
                throw new BadRequestException($"Dish with Guid {id} not found.");
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

        public async Task<bool> CheckToSetRatingAsync(Guid id, string token)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == id);
            if (dish == null)
            {
                throw new NotFoundException($"Dish with Guid {id} not found.");
            }

            var userId = Guid.Parse(JWTTokenService.GetValueFromToken(token, "UserId"));

            var dishInCarts = await
                _dbContext.DishInCarts
                .FirstOrDefaultAsync(dishInCarts => dishInCarts.UserId == userId && dishInCarts.DishId == id && dishInCarts.OrderId != null);

            if (dishInCarts == null)
            {
                return false;
            }

            return true;
        }

        public async Task SetRatingAsync(Guid id, string token, int ratingScore)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == id);
            if (dish == null)
            {
                throw new NotFoundException($"Dish with Guid {id} not found.");
            }

            var userId = Guid.Parse(JWTTokenService.GetValueFromToken(token, "UserId"));
            var user = await _dbContext.Users.FirstAsync(user => user.Id == id);

            if (!await CheckToSetRatingAsync(id, token))
            {
                throw new BadRequestException($"You can't rate a dish with an Guid {id}.");
            }

            var rating = await _dbContext.Ratings.FirstOrDefaultAsync(rating => rating.UserId == userId && rating.DishId == id);
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
        }

        private void SortDishes(DishSorting dishSorting, List<DishDTO> dishes)
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
        }
    }
}
