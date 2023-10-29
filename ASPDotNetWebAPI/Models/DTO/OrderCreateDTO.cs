namespace ASPDotNetWebAPI.Models.DTO
{
    public class OrderCreateDTO
    {
        public DateTime DeliveryTime { get; set; }
        public Guid AddressId { get; set; }
    }
}
