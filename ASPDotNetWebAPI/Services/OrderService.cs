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
        private readonly IAddressService _addressService;
        private readonly int TimeUTC;

        public OrderService(ApplicationDbContext dbContext, IAddressService addressService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _addressService = addressService;
            TimeUTC = int.Parse(configuration.GetValue<string>("TimeZoneSettings:TimeUTC"));
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
                .Where(order => order.UserId == userId)
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
            var regionTimeZone = await _addressService.GetRegionTimeZoneAsync(orderCreateDTO.AddressId);
            var nowTime = DateTime.UtcNow.AddHours(TimeUTC + regionTimeZone.TimeDifferenceWithMoscow);

            if (orderCreateDTO.DeliveryTime < nowTime)
            {
                throw new BadRequestException($"The time in the region {nowTime} of the order must be less than the delivery time {orderCreateDTO.DeliveryTime} of the order!");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with Guid {userId} not found!");
            }

            var dishInCart = _dbContext.DishInCarts
                .Include(dishBasket => dishBasket.Dish)
                .Where(dishBasket => dishBasket.UserId == userId && dishBasket.OrderId == null);

            var countDishInCart = await dishInCart.CountAsync();
            if (countDishInCart < 1)
            {
                throw new BadRequestException($"No dishes were found in the basket of the user with Guid {userId}!"); ;
            }

            var order = await _dbContext.Orders.AddAsync(new Order()
            {
                DeliveryTime = orderCreateDTO.DeliveryTime,
                OrderTime = nowTime,
                AddressId = orderCreateDTO.AddressId,
                Status = Status.InProcess,
                User = user
            });

            double totalPrice = 0.0;
            foreach (var dishBasket in dishInCart)
            {
                dishBasket.Order = order.Entity;
                totalPrice += dishBasket.Dish.Price * dishBasket.Count;
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

// Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxOTAyYjQ3MS03ZTExLTRhNWYtOGVkZS0xYWRiODExYjRmMWIiLCJKVEkiOiI2ODcyOWVjNy04MGVhLTRlNTMtYTJmMS02MDBlMjE0YzI2M2UiLCJuYmYiOjE2OTg0ODkwNTMsImV4cCI6MTY5ODQ5MjY1MywiaWF0IjoxNjk4NDg5MDUzLCJpc3MiOiJISVRzIn0.WTSMH-t3aUutMU7Wz_4ykoFZ_tVrU0VqputkK4pM4Vw
