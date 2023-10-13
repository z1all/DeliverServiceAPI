namespace ASPDotNetWebAPI.Models
{
    public class DishBasket
    {
        public Guid DishInCartId { get; set; }
        public Guid UserId { get; set; }
        public Guid DishId { get; set; }

        public DishInCart DishInCart { get; set; }
        public Dish Dish { get; set; }
        public User User { get; set; }
    }
}
