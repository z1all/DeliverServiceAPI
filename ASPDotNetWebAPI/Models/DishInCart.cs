namespace ASPDotNetWebAPI.Models
{
    public class DishInCart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DishId { get; set; }
        public Guid? OrderId { get; set; }
        public int Count { get; set; }

        public User User { get; set; }
        public Dish Dish { get; set; }
        public Order? Order { get; set; }
    }
}
