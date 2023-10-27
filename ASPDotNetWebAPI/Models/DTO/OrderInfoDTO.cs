using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Models.DTO
{
    public class OrderInfoDTO
    {
        public Guid id { get; set; }
        public DateTime deliveryTime { get; set; }
        public DateTime orderTime { get; set; }
        public Status Status { get; set; }
        public double Price { get; set; }
    }
}
