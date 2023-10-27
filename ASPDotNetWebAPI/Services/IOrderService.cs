using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> GetOrderInfoAsync(Guid userId, Guid orderId);
        Task<List<OrderInfoDTO>> GetOrderInfoListAsync(Guid userId);
        Task CreateOrderFormBasketAsync(Guid userId, OrderCreateDTO orderCreateDTO);
        Task ConfirmOrderDeliveryAsync(Guid userId, Guid orderId);
    }
}
