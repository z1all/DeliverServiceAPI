using ASPDotNetWebAPI.Exceptions;
using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderDTO> GetOrderInfoAsync(Guid userId, Guid orderId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found!");
            }

            var order = await _dbContext.Orders.FirstOrDefaultAsync(order => order.Id == orderId && order.UserId == userId);
            if (order == null)
            {
                throw new NotFoundException($"A user with Guid {userId} does not have an order with Guid {orderId}");
            }

            var listOfDishInCart = await _dbContext.DishInCarts
                .Where(dishInCart => dishInCart.OrderId == order.Id)
                .Select(dishInCart => new DishBasketDTO()
                {
                    Id = dishInCart.Dish.Id,
                    Name = dishInCart.Dish.Name,
                    Price = dishInCart.Dish.Price,
                    TotalPrice = dishInCart.Dish.Price * dishInCart.Count,
                    Amount = dishInCart.Count,
                    Image = dishInCart.Dish.Image
                })
                .ToListAsync();

            return new OrderDTO()
            {
                Id = order.Id,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Status = order.Status,
                Address = order.AddressId,
                Dishes = listOfDishInCart
            };
        }

        public async Task<List<OrderInfoDTO>> GetOrderInfoListAsync(Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found!");
            }

            var listOfOrders = await _dbContext.Orders
                .Where(order => order.Id == userId)
                .Select(order => new OrderInfoDTO()
                {
                    Id = order.Id,
                    DeliveryTime = order.DeliveryTime,
                    OrderTime = order.OrderTime,
                    Status = order.Status,
                    Price = order.Price
                })
                .ToListAsync();

            return listOfOrders;
        }

        public async Task CreateOrderFormBasketAsync(Guid userId, OrderCreateDTO orderCreateDTO)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found!");
            }

            var dishInCart = _dbContext.DishInCarts
                .Where(dishBasket => dishBasket.UserId == userId && dishBasket.OrderId == null);

            var countDishInCart = await dishInCart.CountAsync();
            if (countDishInCart < 1)
            {
                throw new BadRequestException($"No dishes were found in the basket of the user with Guid {userId}!"); ;
            }

            var order = await _dbContext.Orders.AddAsync(new Order()
            {
                DeliveryTime = orderCreateDTO.DeliveryTime,
                OrderTime = DateTime.Now,
                AddressId = orderCreateDTO.AddressId,
                Status = Status.InProcess,
                User = user
            });

            double totalPrice = 0.0;
            foreach (var dishBasket in dishInCart)
            {
                dishBasket.Order = order.Entity;
                totalPrice = dishBasket.Dish.Price * dishBasket.Count;
            }
            order.Entity.Price = totalPrice;

            await _dbContext.SaveChangesAsync();
        }

        public async Task ConfirmOrderDeliveryAsync(Guid userId, Guid orderId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found!");
            }

            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(order => order.Id == orderId && order.UserId == userId);

            if (order == null)
            {
                throw new NotFoundException($"A user with Guid {userId} does not have an order with Guid {orderId}");
            }

            order.Status = Status.Delivered;

            await _dbContext.SaveChangesAsync();
        }
    }
}
