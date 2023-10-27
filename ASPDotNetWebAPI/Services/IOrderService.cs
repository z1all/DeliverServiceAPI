using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> GetOrderInfoAsync(Guid userId, Guid orderId);
        Task<List<OrderInfoDTO>> GetOrderInfoListAsync(Guid userId);
        Task CreatOrderFormBasketAsync(Guid userId);
        Task ConfirmOrderDelivery(Guid userId, Guid orderId);
    }
}
