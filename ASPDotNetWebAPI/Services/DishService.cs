using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using ASPDotNetWebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.HttpResults;

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

        public async Task<DishPagedListDTO> GetDishes(DishCategory?[] category, bool isVegetarian, DishSorting dishSorting, int page)
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

            SortDish(dishSorting, dishes);

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

        public async Task<DishDTO> GetDish(Guid id)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == id);

            if (dish == null)
            {
                throw new BadRequestException($"Dish with Guid {id} not found");
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

        public Task<bool> CheckToSetRating(Guid id, string token)
        {
            throw new NotImplementedException();
        }

        public Task SetRating(Guid id, string token, int ratingScore)
        {
            throw new NotImplementedException();
        }

        private void SortDish(DishSorting dishSorting, List<DishDTO> dishes)
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
