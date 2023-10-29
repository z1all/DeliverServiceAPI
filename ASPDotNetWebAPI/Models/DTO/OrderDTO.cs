using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public Status Status { get; set; }
        public Guid Address { get; set; }
        public List<DishBasketDTO> Dishes { get; set; }
    }
}
