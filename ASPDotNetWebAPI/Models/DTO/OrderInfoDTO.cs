using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class OrderInfoDTO
    {
        public Guid Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public Status Status { get; set; }
        public double Price { get; set; }
    }
}
