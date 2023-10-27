using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task ConfirmOrderDelivery(Guid userId, Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task CreatOrderFormBasketAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO> GetOrderInfoAsync(Guid userId, Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderInfoDTO>> GetOrderInfoListAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
