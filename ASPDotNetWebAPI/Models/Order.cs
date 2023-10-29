using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public double Price { get; set; }
        public Guid AddressId { get; set; }
        public Status Status { get; set; }

        public User User { get; set; }
    }
}
