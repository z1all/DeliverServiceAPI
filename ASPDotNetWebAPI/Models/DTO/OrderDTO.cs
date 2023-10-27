using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class OrderDTO
    {
        public Guid id { get; set; }
        public DateTime deliveryTime { get; set; }
        public DateTime orderTime { get; set; }
        public Status Status { get; set; }
        public Guid Address { get; set; }
        public List<DishInCart> dishes { get; set; }
    }
}
